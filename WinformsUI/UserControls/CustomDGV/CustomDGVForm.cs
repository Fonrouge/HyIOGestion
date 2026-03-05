using Shared;
using Shared.Services.Searching;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Winforms.Theme;
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

        private string _placeholder;
        private dynamic _searchBehavior;
        private bool _filtersConfigured = false;

        private CheckBox _allCategories;

        public CustomDGVForm
        (
            IListFilterSortProvider listTools,
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr
        )
        {
            _listTools = listTools;
            _appSettings = appSettings;
            _transMgr = transMgr;

            InitializeComponent();
            InitializeVariables();
            SetFormAppearence();
            WireCommonEvents();

            // Wire de botones del panel de filtros (Refactorizado sin lambdas)
            btnApplyFilter.Click += BtnApplyFilter_Click;
            btnCleanFilters.Click += BtnCleanFilters_Click;

            panelHorDivider.Visible = false;
            tableLayoutPanelFilters.Visible = false;

            btnShowFilters.Click += BtnShowFilters_Click;
        }

        // ====================== APIS PÚBLICAS ======================

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int LB_SETITEMHEIGHT = 0x01A0;

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

            // MAGIA: 30 es la nueva altura en píxeles (juega con este valor)
            SendMessage(checkedListBoxFilters.Handle, LB_SETITEMHEIGHT, IntPtr.Zero, (IntPtr)24);
            checkedListBoxFilters.Refresh();
        }

        public void ClearFilters()
        {
            for (int i = 0; i < checkedListBoxFilters.Items.Count; i++)
                checkedListBoxFilters.SetItemChecked(i, false);

            _searchBehavior?.ExecuteContainsFilter(new List<string>());
        }

        public void FillDGV<T>(IEnumerable<T> data) where T : IDto
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            _searchBehavior = new SearchBehavior<T>
            (
                dgv: mainDGV,
                searchPresenter: _listTools,
                appSettings: _appSettings,
                transMgr: _transMgr
            );

            _searchBehavior.AttachNewTextBoxAsSearchBar(tbSearchBar, _appSettings.SearchBarPlaceHolder);
            _searchBehavior.UpdateList(data.ToBindingList());

            mainDGV.DataSource = data;

            AddTranslatables();
            ApplyTranslation();
            SetDGVAppearence();
        }

        public void EnsureDgvRowSelection()
        {
            if (mainDGV.Rows.Count > 0 && mainDGV.SelectedRows.Count == 0)
            {
                mainDGV.Rows[0].Selected = true;
                mainDGV.CurrentCell = mainDGV.Rows[0].Cells[0];
            }
        }

        // ====================== INTERNALS ======================

        private void WireCommonEvents()
        {
            mainDGV.SelectionChanged += MainDGV_SelectionChanged;
            this.FormClosed += CustomDGVForm_FormClosed;
        }

        private void InitializeVariables()
        {
            _placeholder = _appSettings.SearchBarPlaceHolder;
            tbSearchBar.Text = _placeholder;
        }

        private void SetFormAppearence()
        {
            DoubleBuffering.TryForAllControls(this.Controls);
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;

            DarkTheme.RedrawBorders = true;
        }

        private void SetDGVAppearence() => GiveMainDataGridViewFormat.Execute(mainDGV);

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<DateTimePicker>(this.Controls, "Text");
            _transMgr.AddParentedObjects<ComboBox>(this.Controls, "");
            _transMgr.AddFormNotify(this);

            _transMgr.AddString("CustomDGVForm.TextBox.Placeholder", _placeholder);

            foreach (DataGridViewColumn column in mainDGV.Columns)
            {
                _transMgr.AddString($"CustomDGVForm.DataGridView.Column.{column.Name}", column.HeaderText);
            }

            _searchBehavior.AttachColumnSelector(cbColumnsName);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation(); //By reflection, DON'T DELETE. 

        private void ApplyTranslation()
        {
            foreach (DataGridViewColumn col in mainDGV.Columns)
            {
                var translatedHeader = _transMgr.GetString($"CustomDGVForm.DataGridView.Column.{col.Name}");
                col.HeaderText = translatedHeader;
            }

            _searchBehavior.UpdatePlaceHolder(_transMgr.GetString("CustomDGVForm.TextBox.Placeholder"));
            _searchBehavior.RefreshColumnsIfChanged();
            _transMgr.Apply();
        }

        // ====================== EVENT HANDLERS (Refactorizados) ======================

        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            if (!_filtersConfigured || checkedListBoxFilters.Items.Count == 0) return;

            // LINQ mágico para extraer los strings de la colección de items chequeados
            var selectedValues = checkedListBoxFilters.CheckedItems
                                                      .Cast<object>()
                                                      .Select(item => item.ToString())
                                                      .ToList();

            _searchBehavior?.ExecuteContainsFilter(selectedValues); //ASDKFJASHDFKAJSDFHJKASDKFJASHDFKAJSDFHJKASDKFJASHDFKAJSDFHJKASDKFJASHDFKAJSDFHJKASDKFJASHDFKAJSDFHJKASDKFJASHDFKAJSDFHJK
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
            if (_searchBehavior != null)
            {
                _searchBehavior.Dispose();
            }
        }

        // ====================== DISPOSE (Seguro contra Memory Leaks) ======================

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 1. Desuscribir todos los eventos cableados a métodos
                btnApplyFilter.Click -= BtnApplyFilter_Click;
                btnCleanFilters.Click -= BtnCleanFilters_Click;
                btnShowFilters.Click -= BtnShowFilters_Click;

                if (mainDGV != null)
                {
                    mainDGV.SelectionChanged -= MainDGV_SelectionChanged;
                }

                this.FormClosed -= CustomDGVForm_FormClosed;

                // 2. Liberar delegados externos
                SelectedRowChanged = null;

                // 3. Dispose de recursos dinámicos
                if (_searchBehavior != null)
                {
                    _searchBehavior.Dispose();
                    _searchBehavior = null;
                }

                // IMPORTANTE: Si ITranslatableControlsManager guarda este form, 
                // descomenta la siguiente línea si tienes el método implementado:
                // _transMgr?.RemoveFormNotify(this);
            }

            base.Dispose(disposing);
        }
    }
}