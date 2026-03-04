using Shared;
using Shared.Services.Searching;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.SearchBar;

namespace WinformsUI.UserControls.CustomDGV
{
    public partial class CustomDGVForm : Form
    {
        public event EventHandler<IDto> SelectedRowChanged;

        public readonly IListFilterSortProvider _listTools;
        public readonly IApplicationSettings _appSettings;
        public readonly ITranslatableControlsManager _transMgr;

        private string _placeholder;
        private dynamic _searchBehavior;
        private bool _filtersConfigured = false;

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
            SetDGVAppearence();
            WireCommonEvents();

            // Wire de botones del panel de filtros
            btnApplyFilter.Click += BtnApplyFilter_Click;
            btnCleanFilters.Click += (s, e) => ClearFilters();


            panelHorDivider.Visible = false;
            tableLayoutPanelFilters.Visible = false;

            btnShowFilters.Click += (s, e) =>
            {
                panelHorDivider.Visible = !tableLayoutPanelFilters.Visible;
                tableLayoutPanelFilters.Visible = !tableLayoutPanelFilters.Visible;
            };
            
        }


        // ====================== APIS PÚBLICAS ======================

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
        }

        public void ClearFilters()
        {
            for (int i = 0; i < checkedListBoxFilters.Items.Count; i++)
                checkedListBoxFilters.SetItemChecked(i, false);

            _searchBehavior?.ExecuteContainsFilter("Categories", string.Empty);
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
            mainDGV.SelectionChanged += (sender, e) =>
            {
                if (mainDGV.SelectedRows.Count > 0)
                {
                    SelectedRowChanged?.Invoke(this, mainDGV.SelectedRows[0].DataBoundItem as IDto);
                }
            };
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

        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            if (!_filtersConfigured || checkedListBoxFilters.Items.Count == 0) return;

            var selected = checkedListBoxFilters.CheckedItems
                .Cast<object>()
                .Select(x => x.ToString())
                .ToList();

            string filterValue = selected.Any() ? string.Join(" ", selected) : string.Empty;

            _searchBehavior?.ExecuteContainsFilter("Categories", filterValue);
        }


    }
}