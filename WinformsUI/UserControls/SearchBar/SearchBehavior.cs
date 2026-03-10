using Shared;
using Shared.Services.Searching;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Helpers;
using WinformsUI.Infrastructure.Translations;
using System.Reflection;

namespace WinformsUI.UserControls.SearchBar
{
    /// <summary>
    /// Comportamiento de búsqueda reutilizable para DataGridView.
    /// Se puede adjuntar a cualquier TextBox (placeholder incluido)
    /// y opcionalmente a un ComboBox para elegir columna.
    /// </summary>
    public sealed class SearchBehavior<T> : IDisposable where T : IDto
    {
        // --- Dependencias / UI ---
        private readonly DataGridView _dgv;
        private TextBox _tb;
        private ComboBox _columnSelector; // opcional
        private readonly IListFilterSortProvider _searchPresenter;
        private ITranslatableControlsManager _transMgr { get; set; }
        private readonly IApplicationSettings _appSettings;

        // --- Datos / Estado Base ---
        private BindingList<T> _items { get; set; }
        private readonly BindingSource _bs = new BindingSource();
        private string _placeholder;
        private bool _placeholderActive = true;
        private bool _isTranslating = false;
        private bool _sortAsc = true;

        // --- Estado de Filtros Avanzados ---
        private List<string> _advancedFilterValues = new List<string>();
        private bool _advancedMatchAll = false;

        // --- Timer (debounce) ---
        private readonly Timer _debounce = new Timer();

        // --- Firma de columnas (para rebind seguro) ---
        private string _lastColsSig = "";
        private bool _rebindingCols = false;

        // Proxy para traducir el placeholder (opcional)
        private Control _placeholderProxy;

        // --- Estado de Filtro por Rango de Fechas ---
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private string _dateColumnName;
        private List<PropertyInfo> _dateProperties; // cache para performance

        /// <summary>Milisegundos de debounce (default 400).</summary>
        public int DebounceMs
        {
            get => _debounce.Interval;
            set => _debounce.Interval = Math.Max(0, value);
        }

        /// <summary>
        /// Crea el helper de búsqueda. 
        /// </summary>      
        public SearchBehavior
        (
            DataGridView dgv,
            IListFilterSortProvider listFilterSort,
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr = null,
            int debounceMs = 400
        )
        {
            _dgv = dgv ?? throw new ArgumentNullException(nameof(dgv));
            _searchPresenter = listFilterSort ?? throw new ArgumentNullException(nameof(listFilterSort));
            _transMgr = transMgr;
            _appSettings = appSettings;

            _placeholder = _appSettings.SearchBarPlaceHolder;

            _debounce.Interval = Math.Max(0, debounceMs);
            _debounce.Tick += (_, __) =>
            {
                _debounce.Stop();
                if (!_isTranslating && !_placeholderActive)
                    ExecuteSearch();
            };

            _dgv.ColumnHeaderMouseClick -= DgvOnHeaderClick;
            _dgv.ColumnHeaderMouseClick += DgvOnHeaderClick;
            _transMgr?.AddString("ColumnSelectorForSearchbar.all", _appSettings.ComboBoxPlaceholder); 
            SetupTranslationAndPlaceholder();

            // Cache de propiedades DateTime (solo una vez)
            _dateProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                .ToList();
        }

        /// <summary>
        /// Adjunta un TextBox como caja de búsqueda y aplica placeholder.
        /// </summary>
        public void AttachNewTextBoxAsSearchBar(TextBox tb, string placeholder)
        {
            if (tb == null) throw new ArgumentNullException(nameof(tb));

            // Desuscribir el anterior (si había)
            if (_tb != null) UnwireTextBox(_tb);

            _tb = tb;

            // Actualizar placeholder (con traducción si corresponde)
            if (!string.IsNullOrWhiteSpace(placeholder))
            {
                _placeholder = placeholder;

                if (_transMgr != null)
                {
                    _placeholderProxy = new Control();
                    _placeholderProxy.Text = _placeholder;
                    _transMgr.AddSingleObject(_placeholderProxy, "Text");
                    _transMgr.Apply();
                    _placeholder = _placeholderProxy.Text;
                }
            }

            // Suscribir eventos del TextBox y aplicar placeholder inicial
            WireTextBox(_tb);
            ApplyPlaceholder();
        }

        public void UpdatePlaceHolder(string newPH)
        {
            _placeholder = newPH;
            _tb.Text = _placeholder ?? "Error";
        }

        /// <summary>
        /// Adjunta un ComboBox para seleccionar columna. Si no se adjunta, se filtra por todas.
        /// </summary>
        public void AttachColumnSelector(ComboBox columnSelector)
        {
            _columnSelector = columnSelector;

            if (_columnSelector == null) return;

            PopulateColumnSelector();
            _columnSelector.SelectionChangeCommitted -= ColumnSelectorOnSelectionChangeCommitted;
        }

        // (opcional pero útil y mínimo) Overload si ya tenés una BindingList<T>
        public void UpdateList(BindingList<T> items)
        {
            _items = items ?? new BindingList<T>();
            _dgv.DataSource = _items;
            ExecuteSearch(); // Ejecutamos la búsqueda para re-aplicar filtros existentes sobre la nueva lista
        }

        /// <summary>
        /// Si cambian las columnas del DGV, rebindea el ComboBox respetando la selección previa.
        /// </summary>
        public void RefreshColumnsIfChanged()
        {
            if (_columnSelector == null) return;

            var sig = ComputeColumnsSignature();
            if (sig == _lastColsSig) return;

            _rebindingCols = true;

            try
            {
                var previousIndex = _columnSelector.SelectedIndex;
                PopulateColumnSelector();
                _columnSelector.SelectedIndex =
                    (previousIndex >= 0 && previousIndex < _columnSelector.Items.Count) ? previousIndex : 0;
                _lastColsSig = sig;
            }
            finally { _rebindingCols = false; }
        }

        // =========================
        // PIPELINE DE FILTRADO (EL MOTOR)
        // =========================

        /// <summary>
        /// Actualiza el estado de los filtros avanzados y dispara la tubería de filtrado.
        /// </summary>
        public void ExecuteContainsFilter(List<string> filterValues, bool matchAll = false)
        {
            _advancedFilterValues = filterValues ?? new List<string>();
            _advancedMatchAll = matchAll;
            ApplyAllFilters();
        }

        /// <summary>
        /// Aplica filtro por rango de fechas (complementario con los demás filtros).
        /// Si columnName es null/empty/"0" → busca en CUALQUIER propiedad DateTime del DTO (lógica OR).
        /// </summary>
        public void ExecuteDateRangeFilter(DateTime from, DateTime upTo, string columnName = null)
        {
            _dateFrom = from.Date;
            _dateTo = upTo.Date.AddDays(1).AddTicks(-1); // inclusive hasta fin del día
            _dateColumnName = string.IsNullOrWhiteSpace(columnName) || columnName == "0"
                ? null
                : columnName.Trim();

            ApplyAllFilters();
        }

        /// <summary>
        /// Limpia el filtro de fechas.
        /// </summary>
        public void ClearDateFilter()
        {
            _dateFrom = null;
            _dateTo = null;
            _dateColumnName = null;
            ApplyAllFilters();
        }

        /// <summary>
        /// Dispara la tubería de filtrado (usualmente llamado por el debounce del SearchBar).
        /// </summary>
        public void ExecuteSearch()
        {
            ApplyAllFilters();
        }

        /// <summary>
        /// Método maestro que encadena los filtros avanzados y la búsqueda por texto.
        /// </summary>
        private void ApplyAllFilters()
        {
            if (_items == null || !_items.Any()) return;

            IEnumerable<T> query = _items;

            // ==========================================
            // ETAPA 1: Filtros Avanzados (Categorías / Atributos)
            // ==========================================
            if (_advancedFilterValues != null && _advancedFilterValues.Count > 0)
            {
                var stringProperties = typeof(T).GetProperties()
                                                .Where(p => p.PropertyType == typeof(string) && p.CanRead)
                                                .ToList();

                query = query.Where(item =>
                {
                    if (_advancedMatchAll)
                    {
                        // LÓGICA ESTRICTA (AND)
                        return _advancedFilterValues.All(filter =>
                            stringProperties.Any(prop =>
                                (prop.GetValue(item) as string)?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0));
                    }
                    else
                    {
                        // LÓGICA PERMISIVA (OR)
                        return _advancedFilterValues.Any(filter =>
                            stringProperties.Any(prop =>
                                (prop.GetValue(item) as string)?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0));
                    }
                });
            }

            // ==========================================
            // ETAPA 2: Filtro por Rango de Fechas (NUEVO)
            // ==========================================
            query = ApplyDateFilter(query);

            // ==========================================
            // ETAPA 3: Búsqueda por Texto (SearchBar)
            // ==========================================
            var txt = _tb?.Text ?? string.Empty;
            if (!_placeholderActive && !string.IsNullOrWhiteSpace(txt) && txt != _placeholder)
            {
                string columnName = "0";
                if (_columnSelector != null && _columnSelector.SelectedIndex > 0)
                    columnName = (_columnSelector.SelectedItem as ColumnItem)?.DataPropertyName ?? "0";

                // Le pasamos al presenter el query temporal
                query = _searchPresenter.Filter(query.ToList(), txt, columnName);
            }

            // ==========================================
            // ETAPA 4: Refrescar UI
            // ==========================================
            _bs.DataSource = query.ToList();
            _dgv.DataSource = _bs;
        }


        // =========================
        // Internals (Eventos y UI)
        // =========================

        private void SetupTranslationAndPlaceholder()
        {
            if (_transMgr != null)
            {
                _placeholderProxy = new Control { Text = _placeholder };
                _transMgr.AddSingleObject(_placeholderProxy, "Text");
                _transMgr.Apply();
                _placeholder = _placeholderProxy.Text;

                // Si ya hay textbox conectado y está en modo placeholder, refrescar su texto.
                if (_tb != null && _placeholderActive)
                {
                    _tb.Text = _placeholder;
                    _tb.ForeColor = SystemColors.GrayText;
                }
            }
            ApplyPlaceholder();
        }

        private void ApplyPlaceholder()
        {
            _placeholderActive = true;
            if (_tb == null) return;
            _tb.Text = _placeholder;

            TextBoxPlaceholder.Apply
            (
                tb: _tb,
                isPlaceholder: true,
                isPassword: false,
                normalForeColor: DarkTheme.GetCurrentPalette().TextSecondary,
                placeholderForeColor: null,
                fontSize: 11.00f
            );
        }

        private void RemovePlaceholder()
        {
            _placeholderActive = false;
            if (_tb == null) return;
            _tb.Clear();

            TextBoxPlaceholder.Apply
            (
                tb: _tb,
                isPlaceholder: false, // Corrección: acá va false porque estamos sacando el placeholder
                isPassword: false,
                normalForeColor: DarkTheme.GetCurrentPalette().TextPrimary, // Corrección sugerida de UI
                placeholderForeColor: null,
                fontSize: 11.00f
            );
        }

        private void WireTextBox(TextBox tb)
        {
            tb.Enter += Tb_Enter;
            tb.Leave += Tb_Leave;
            tb.KeyDown += Tb_KeyDown;
            tb.TextChanged += Tb_TextChanged;

            // Asegurar buen comportamiento de navegación desde el inicio
            TextBoxPlaceholder.Apply(tb: _tb, isPlaceholder: true, isPassword: false, normalForeColor: DarkTheme.GetCurrentPalette().TextSecondary, placeholderForeColor: null, fontSize: 11.00f);
        }

        private void UnwireTextBox(TextBox tb)
        {
            tb.Enter -= Tb_Enter;
            tb.Leave -= Tb_Leave;
            tb.KeyDown -= Tb_KeyDown;
            tb.TextChanged -= Tb_TextChanged;
        }

        private void Tb_Enter(object sender, EventArgs e)
        {
            if (_placeholderActive) RemovePlaceholder();
        }

        private void Tb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_tb.Text) || _tb.Text == _placeholder)
                ApplyPlaceholder();
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            _debounce.Stop();

            if (!_isTranslating && !_placeholderActive)
                ExecuteSearch();
        }

        private void Tb_TextChanged(object sender, EventArgs e)
        {
            if (_isTranslating || _placeholderActive) return;

            _debounce.Stop();
            _debounce.Start();
        }

        private void DgvOnHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _sortAsc = !_sortAsc;
            var colName = _dgv.Columns[e.ColumnIndex]?.DataPropertyName;
            if (string.IsNullOrWhiteSpace(colName)) return;

            // Ordena respetando los filtros actuales (lo que se ve en pantalla)
            var current = (_bs.List as IEnumerable<T>)?.ToList() ?? _items.ToList();
            var sorted = _searchPresenter.Sort(current, colName, _sortAsc).ToList();

            _bs.DataSource = sorted;
            _dgv.DataSource = _bs;
        }

        private void ColumnSelectorOnSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_rebindingCols) return;
            _debounce.Stop();
            if (!_isTranslating && !_placeholderActive)
                ExecuteSearch();
        }

        private void PopulateColumnSelector()
        {
            if (_columnSelector == null) return;

            _columnSelector.SelectedIndexChanged -= ColumnSelectorOnSelectionChangeCommitted;

            // Texto visible: traducido (no afecta la lógica)
            string allText = _transMgr != null ? _transMgr.GetString("ColumnSelectorForSearchbar.all") : "Todas";

            var cols = _dgv.Columns.Cast<DataGridViewColumn>().Select
            (
                c => new ColumnItem
                {
                    HeaderText = c.HeaderText ?? c.Name ?? "",
                    DataPropertyName = !string.IsNullOrWhiteSpace(c.DataPropertyName) ? c.DataPropertyName : c.Name
                }
            )
            .ToList();

            // Posición 0: todas las columnas
            cols.Insert(0, new ColumnItem { HeaderText = allText, DataPropertyName = "0" });

            _columnSelector.DropDownStyle = ComboBoxStyle.DropDownList; // evita texto libre
            _columnSelector.DataSource = cols;
            _columnSelector.DisplayMember = nameof(ColumnItem.HeaderText);
            _columnSelector.ValueMember = nameof(ColumnItem.DataPropertyName);

            _columnSelector.SelectedIndexChanged += ColumnSelectorOnSelectionChangeCommitted;

            _lastColsSig = ComputeColumnsSignature();
        }

        private string ComputeColumnsSignature()
        {
            var parts = _dgv.Columns.Cast<DataGridViewColumn>()
                .Select(c => $"{c.Name}|{c.DataPropertyName}|{c.HeaderText}|{c.Visible}")
                .ToArray();
            return $"{_dgv.Columns.Count}::{string.Join("§", parts)}";
        }

        private IEnumerable<T> ApplyDateFilter(IEnumerable<T> source)
        {
            if (_dateFrom == null || _dateTo == null || !_dateProperties.Any())
                return source;

            return source.Where(item =>
            {
                if (!string.IsNullOrEmpty(_dateColumnName))
                {
                    // === Columna específica ===
                    var prop = _dateProperties.FirstOrDefault(p =>
                        string.Equals(p.Name, _dateColumnName, StringComparison.OrdinalIgnoreCase));

                    return prop != null && IsInRange(prop.GetValue(item));
                }
                else
                {
                    // === Cualquier columna de fecha (OR) ===
                    return _dateProperties.Any(prop => IsInRange(prop.GetValue(item)));
                }
            });
        }

        private bool IsInRange(object value)
        {
            if (value == null) return false;

            DateTime dt;

            if (value is DateTime)
            {
                dt = (DateTime)value;
            }
            else
            {
                DateTime? nullable = (DateTime?)value;
                dt = nullable.Value;   // siempre tiene valor porque viene de reflexión
            }

            return dt >= _dateFrom.Value && dt <= _dateTo.Value;
        }

        private sealed class ColumnItem
        {
            public string HeaderText { get; set; }
            public string DataPropertyName { get; set; }
        }

        public void Dispose()
        {
            _debounce?.Stop();
            _debounce?.Dispose();

            if (_tb != null) UnwireTextBox(_tb);
            _dgv.ColumnHeaderMouseClick -= DgvOnHeaderClick;

            if (_columnSelector != null)
                _columnSelector.SelectionChangeCommitted -= ColumnSelectorOnSelectionChangeCommitted;
        }
    }
}