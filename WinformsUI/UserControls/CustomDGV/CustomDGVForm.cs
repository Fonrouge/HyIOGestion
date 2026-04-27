using Shared;
using Shared.Services.Searching;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Shortcuts;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.SearchBar;

namespace WinformsUI.UserControls.CustomDGV
{
    public partial class CustomDGVForm : Form, IDisposable
    {
       
        public event EventHandler<IDto> SelectedRowChanged;

        public readonly IListFilterSortProvider _listTools;
        public readonly IApplicationSettings _appSettings;
        public readonly ITranslatableControlsManager _transMgr;

        private readonly DGVCellsStyle _styleHelper = new DGVCellsStyle();
        private ShortcutManager _shortcutMgr;

        private string _placeholder;
        private dynamic _searchBehavior;
        private bool _filtersConfigured = false;
        private Type _entityType;
        private bool _firstOpen = true;

        private sealed class DateColumnItem
        {
            public string HeaderText { get; set; }
            public string PropertyName { get; set; }
        }

        public CustomDGVForm
        (
            IListFilterSortProvider listTools,
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr
        )
        {
            SuspendLayout();

            _listTools = listTools;
            _appSettings = appSettings;
            _transMgr = transMgr;

            InitializeComponent();
            InitializeVariables();
            SetFormAppearence();
            WireCommonEvents();

            btnApplyFilter.Click += BtnApplyFilter_Click;
            btnCleanFilters.Click += BtnCleanFilters_Click;
            btnShowFilters.Click += BtnShowFilters_Click;

            panelHorDivider.Visible = false;
            tableLayoutPanelFilters.Visible = false;
            _styleHelper.SetDGV(mainDGV);
            SetShortcuts();
            ResumeLayout();
            
        }
        public DataGridView MainDGV => mainDGV;
        public TextBox AskForSearchBar()
        {
            tableLayoutPanelSearchControls.Controls.Remove(tbSearchBar);
            tableLayoutPanelSearchControls.Visible = false;
            return tbSearchBar;
        }
        public void ReturnSearchBar(TextBox sb)
        {
            tbSearchBar = sb;
            tableLayoutPanelSearchControls.Controls.Add(tbSearchBar, 2, 0);
            tableLayoutPanelSearchControls.Visible = true;
        }
        bool searchBarIsGone = false;

        private void SetShortcuts() => _shortcutMgr = ShortcutManager.Attach(this)
               .BindWheelZoom(() => this.ZoomIn(), () => this.ZoomOut())
               .Add("Ctrl+H", () => ToggleSearchBar())
               .Add("Ctrl+F", () => ToggleFiltersPanel())
            ;


        // ====================== APIS PÚBLICAS ======================
        /// <summary>
        /// Recibe el listado que hará de Source para un CheckedListBox en pos de que el usuario pueda elegir por cuáles categorías filtrar.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="items"></param>
        /// <param name="displayMember"></param>
        /// <param name="showPanelImmediately"></param>
        public void ConfigureFilters<TItem>
        (
            IEnumerable<TItem> items,
            string displayMember = "Name",
            bool showPanelImmediately = true
        )
        {
            checkedListBoxFilters.Items.Clear();
            checkedListBoxFilters.DisplayMember = displayMember;
            foreach (var item in items ?? Enumerable.Empty<TItem>())
                checkedListBoxFilters.Items.Add(item);

            _filtersConfigured = true;
            ChooseCheckedListBoxItemsHeight(24);
        }

        // ====================== APIs DE PERSONALIZACIÓN ======================

        public void SwitchHorizontalDividerVisibility(bool turnOn) => horizontalDividerPanel.Visible = turnOn;
        public void ToggleSearchBar() => tableLayoutPanelSearchControls.Visible = !tableLayoutPanelSearchControls.Visible;
        public bool IsToggleSearchBarVisible() => tableLayoutPanelSearchControls.Visible;
        public void ToggleFiltersPanel() => BtnShowFilters_Click(this, EventArgs.Empty);

        // ---------- ZOOM ----------
        public void ZoomIn() => _styleHelper.ZoomGrid(mainDGV, 1.0f);

        public void ZoomOut() => _styleHelper.ZoomGrid(mainDGV, -1.0f);

        // ---------- ALTURAS ----------
        public void NudgeRowHeight(int deltaPx)
            => _styleHelper.NudgeHeights(deltaPx);

        public void NudgeHeaderHeight(int deltaPx)
            => _styleHelper.NudgeHeights(0, alsoHeader: true, deltaHeaderPx: deltaPx);

        public void AutoFitRowAndHeader(bool alsoHeader = true, int extraRowPx = 2, int extraHeaderPx = 2)
            => _styleHelper.ApplyRecommendedRowHeight(alsoHeader, extraRowPx, extraHeaderPx);

        // ---------- ANCHOS DE COLUMNAS ----------
        public void NudgeColumnWidth(int deltaPx, bool onlySelected = false)
            => _styleHelper.NudgeColumnWidths(deltaPx, onlySelected);

        public void AutoFitColumns(bool onlySelected = false, int sampleRows = 200, bool includeHeader = true, int extraPx = 12)
            => _styleHelper.ApplyRecommendedColumnWidths(onlySelected, sampleRows, includeHeader, extraPx);

        // ---------- ALINEACIÓN ----------
        public void AlignCellsLeft(bool onlySelected = false, bool includeHeader = false)
            => _styleHelper.AlignCellsLeft(onlySelected, includeHeader);

        public void AlignCellsCenter(bool onlySelected = false, bool includeHeader = false)
            => _styleHelper.AlignCellsCenter(onlySelected, includeHeader);

        public void AlignCellsRight(bool onlySelected = false, bool includeHeader = false)
            => _styleHelper.AlignCellsRight(onlySelected, includeHeader);

        // ---------- ESTADOS (para habilitar/deshabilitar botones) ----------
        public (bool canGrowRows, bool canShrinkRows, bool canGrowHeader, bool canShrinkHeader)
            GetRowHeaderNudgeState()
            => _styleHelper.GetCanNudgeState();

        public (bool canWider, bool canNarrower)
            GetColumnNudgeState(bool onlySelected = false)
                     => _styleHelper.GetCanNudgeColumnsState(onlySelected: onlySelected);

        // ---------- BORDES (GRILLA) ----------
        public void SetAllBorders()
            => _styleHelper.SetGridAllBorders();

        public void SetVerticalBordersOnly()
            => _styleHelper.SetGridVerticalBordersOnly();

        public void SetNoBorders()
            => _styleHelper.SetGridNoBorders();

        // ---------- COPIAR ----------
        public void CopyAsText()
            => _styleHelper.CopySelectedToClipboardAsText(includeHeaders: true);

        public void CopyAsCell()
            => _styleHelper.CopySelectedToClipboardStandard();

        public void FocusSearchBar() => tbSearchBar.Focus();
        public void FocusFirstDGVRow() => EnsureDgvRowSelection();
        public void OpenFiltersPanel()
        {
            BtnShowFilters_Click(this, EventArgs.Empty);
            if (tableLayoutPanelFilters.Visible)
                rbAtLeastOneCategory.Focus();

            else
                FocusFirstDGVRow();
        }

  

        public void RepaintSearchBarPlaceHolder(Color c)
        {
            if (_searchBehavior != null && tbSearchBar != null)
                tbSearchBar.ForeColor = DarkTheme.GetCurrentPalette().TextSecondary;

            //_searchBehavior.UpdatePlaceHolder(_transMgr.GetString("CustomDGVForm.TextBox.Placeholder"));
        }




        #region For choose CheckedListBox items height
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int LB_SETITEMHEIGHT = 0x01A0;

        private void ChooseCheckedListBoxItemsHeight(int desiredHeight)
        {
            SendMessage(checkedListBoxFilters.Handle, LB_SETITEMHEIGHT, IntPtr.Zero, (IntPtr)desiredHeight);
            checkedListBoxFilters.Refresh();
        }
        #endregion

        public void ClearFilters()
        {
            for (int i = 0; i < checkedListBoxFilters.Items.Count; i++)
                checkedListBoxFilters.SetItemChecked(i, false);

            dateTimePickerSince.Value = DateTime.Today.AddMonths(-1);
            dateTimePickerUpTo.Value = DateTime.Today;

            _searchBehavior?.ExecuteContainsFilter(new List<string>());
            _searchBehavior?.ClearDateFilter();
        }

        public void FillDGV<T>(IEnumerable<T> data) where T : IDto
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            _entityType = typeof(T);

            _searchBehavior = new SearchBehavior<T>
            (
                dgv: mainDGV,
                listFilterSort: _listTools,
                appSettings: _appSettings,
                transMgr: _transMgr
            );

            _searchBehavior.AttachNewTextBoxAsSearchBar(tbSearchBar, _appSettings.SearchBarPlaceHolder);
            _searchBehavior.UpdateList(data.ToBindingList());

            mainDGV.DataSource = data;

            AddTranslatables();
            ApplyTranslation();

            if (_firstOpen)
                SetDGVAppearence();

            _firstOpen = false;
        }

        /// <summary>
        /// Refresca el ComboBox de filtro por fecha (headers traducidos + preserva selección)
        /// </summary>
        private void RefreshDateFilterComboBox()
        {
            if (_entityType == null) return;

            // Guardar selección actual por PropertyName
            string selectedProp = (cbColumnsNameFilterDate.SelectedItem as DateColumnItem)?.PropertyName;

            cbColumnsNameFilterDate.Items.Clear();

            var dateProperties = _entityType.GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                .ToList();

            string allText = _transMgr?.GetString("CustomDGVForm.DateFilter.All") ?? "Todas";

            // Opción "Todas"
            cbColumnsNameFilterDate.Items.Add(new DateColumnItem
            {
                HeaderText = allText,
                PropertyName = null
            });

            // Propiedades DateTime
            foreach (var prop in dateProperties)
            {
                string displayText = _transMgr?.GetString($"CustomDGVForm.DataGridView.Column.{prop.Name}")
                                  ?? prop.Name;

                cbColumnsNameFilterDate.Items.Add(new DateColumnItem
                {
                    HeaderText = displayText,
                    PropertyName = prop.Name
                });
            }

            cbColumnsNameFilterDate.DisplayMember = nameof(DateColumnItem.HeaderText);
            cbColumnsNameFilterDate.ValueMember = nameof(DateColumnItem.PropertyName);

            // Restaurar selección anterior (si aún existe)
            if (!string.IsNullOrEmpty(selectedProp))
            {
                var match = cbColumnsNameFilterDate.Items.Cast<DateColumnItem>()
                    .FirstOrDefault(i => i.PropertyName == selectedProp);
                if (match != null)
                    cbColumnsNameFilterDate.SelectedItem = match;
                else
                    cbColumnsNameFilterDate.SelectedIndex = 0;
            }
            else
                cbColumnsNameFilterDate.SelectedIndex = 0;
        }

        /// <summary>
        /// Método profiláctico que asegura que siempre haya una fila seleccionada en el DataGridView (si tiene filas), para evitar excepciones sobre objetos potencialmente nulos.
        /// </summary>
        public void EnsureDgvRowSelection()
        {
            if (mainDGV.Rows.Count > 0)
            {
                if (mainDGV.SelectedRows.Count == 0)
                {
                    mainDGV.Rows[0].Selected = true;
                    mainDGV.CurrentCell = mainDGV.Rows[0].Cells[0];
                }

                mainDGV.Focus();


                // Task.Delay(1000);
                // tbSearchBar.Enabled = true;
                // tbSearchBar.Enabled = false;
            }
        }

        // ====================== INTERNALS ======================
        private void WireCommonEvents()
        {
            mainDGV.SelectionChanged += MainDGV_SelectionChanged;
            this.FormClosed += CustomDGVForm_FormClosed;
            checkedListBoxFilters.MouseLeave += CheckedListBoxMouseLeave;
        }

        /// <summary>
        /// Deselecciona cualquier item del CheckedListBox cuando el mouse sale de su área, para coherencia visual con una aplicación "moderna".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedListBoxMouseLeave(object sender, EventArgs e) => checkedListBoxFilters.SelectedIndex = -1;

        private void InitializeVariables() => _placeholder = _appSettings.SearchBarPlaceHolder;

        private void SetFormAppearence()
        {
            DoubleBuffering.TryForAllControls(this.Controls);
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            DarkTheme.RedrawBorders = true;
        }

        private void SetDGVAppearence() => DgvFormat.Apply(mainDGV);

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<DateTimePicker>(this.Controls, "Text");
            _transMgr.AddParentedObjects<RadioButton>(this.Controls, "Text");
            _transMgr.AddParentedObjects<ComboBox>(this.Controls, "");
            _transMgr.AddFormNotify(this);


            _transMgr.AddString("CustomDGVForm.TextBox.Placeholder", _placeholder);
            _transMgr.AddString("CustomDGVForm.DateFilter.All", "Todas");

            foreach (DataGridViewColumn column in mainDGV.Columns)
            {
                _transMgr.AddString($"CustomDGVForm.DataGridView.Column.{column.Name}", column.HeaderText);
            }

            _searchBehavior.AttachColumnSelector(cbColumnsNameSearch);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();

        private void ApplyTranslation()
        {
            foreach (DataGridViewColumn col in mainDGV.Columns)
            {
                col.HeaderText = _transMgr.GetString($"CustomDGVForm.DataGridView.Column.{col.Name}");
            }

            _searchBehavior.UpdatePlaceHolder(_transMgr.GetString("CustomDGVForm.TextBox.Placeholder"));

            _searchBehavior.RefreshColumnsIfChanged();
            RefreshDateFilterComboBox();

            _transMgr.Apply();
        }

        // ====================== EVENT HANDLERS ======================
        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            if (!_filtersConfigured || checkedListBoxFilters.Items.Count == 0) return;

            // Categorías (usando GetItemText para que funcione siempre)
            var selectedValues = checkedListBoxFilters.CheckedItems
                .Cast<object>()
                .Select(item => checkedListBoxFilters.GetItemText(item))
                .ToList();

            _searchBehavior.ExecuteContainsFilter(selectedValues, rbAllCategories.Checked);

            // Filtro de fechas: SOLO se aplica si el panel está visible
            // (esto es clave para que la primera vez no filtre por fecha)
            if (tableLayoutPanelFilters.Visible)
            {
                string selectedColumn = null;

                if (cbColumnsNameFilterDate.SelectedItem is DateColumnItem dcItem &&
                    !string.IsNullOrEmpty(dcItem.PropertyName))
                {
                    selectedColumn = dcItem.PropertyName;
                }
                _searchBehavior.ExecuteDateRangeFilter(
                    dateTimePickerSince.Value,
                    dateTimePickerUpTo.Value,
                    selectedColumn);
            }
            else
            {
                _searchBehavior.ClearDateFilter();   // asegura lista completa cuando el panel está oculto
            }
        }

        private void BtnCleanFilters_Click(object sender, EventArgs e) => ClearFilters();

        private void BtnShowFilters_Click(object sender, EventArgs e)
        {
            panelHorDivider.Visible = !tableLayoutPanelFilters.Visible;
            tableLayoutPanelFilters.Visible = !tableLayoutPanelFilters.Visible;
        }

        private void MainDGV_SelectionChanged(object sender, EventArgs e)
        {
            if (mainDGV.SelectedRows.Count > 0)
            {
                SelectedRowChanged?.Invoke(this, mainDGV.SelectedRows[0].DataBoundItem as IDto);
            }
        }

        private void CustomDGVForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _searchBehavior?.Dispose();
        }

        // ====================== DISPOSE ======================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                btnApplyFilter.Click -= BtnApplyFilter_Click;
                btnCleanFilters.Click -= BtnCleanFilters_Click;
                btnShowFilters.Click -= BtnShowFilters_Click;

                if (mainDGV != null)
                    mainDGV.SelectionChanged -= MainDGV_SelectionChanged;

                this.FormClosed -= CustomDGVForm_FormClosed;

                SelectedRowChanged = null;
                _searchBehavior?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}