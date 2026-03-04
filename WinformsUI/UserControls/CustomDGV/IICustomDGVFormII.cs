using Shared;
using Shared.Services.Searching;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls.SearchBar;


namespace WinformsUI.UserControls.CustomDGV
{
    public partial class IICustomDGVFormII : Form 
    {

        public event EventHandler<IDto> SelectedRowChanged;
        public readonly IListFilterSortProvider _listTools;
        public readonly IApplicationSettings _appSettings;
        public readonly ITranslatableControlsManager _transMgr;
        private string _placeholder;
        private dynamic _searchBehavior;

        public IICustomDGVFormII
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

            mainDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //Debug           
        }

        //Public APIS

        /// <summary>
        /// Main API. Se carga un DataGridView la lista lista de DTOs enviada. El formulario se encarga de configurar la búsqueda, traducciones y demás funcionalidades comunes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void FillDGV<T>(IEnumerable<T> data) where T : IDto
        {
            if (data == null) throw new ArgumentNullException("data cannot be empty"); //Future "early return"

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


        //Privates / Internals
        private void WireCommonEvents()
        {
            mainDGV.SelectionChanged += (sender, e) =>
            {
                if (mainDGV.SelectedRows.Count > 0)
                {
                    SelectedRowChanged?.Invoke(this, mainDGV.SelectedRows[0].DataBoundItem as IDto);
                }

                //En caso de ser necesario, más eventos acá.
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

      //      DarkTheme.RedrawBorders = true;
        }

        private void SetDGVAppearence() => GiveMainDataGridViewFormat.Execute(mainDGV);

        private void AddTranslatables()  //Se desuscribe en la clase padre BaseManagementForm FormClosed() => _transMgr.RemoveFormNotify(this);
        {
            //Labels y botones comunes
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<DateTimePicker>(this.Controls, "Text");


            //Controles con tratamiento especial: ComboBox
            _transMgr.AddParentedObjects<ComboBox>(this.Controls, "");


            //Se agrega al notificador que después será invocado por reflexión (por eos pueden verse "0 referencias" en cada uno de los llamados)
            _transMgr.AddFormNotify(this);


            //String sueltos sin un objeto que necesariamente lo contenga
            _transMgr.AddString("CustomDGVForm.TextBox.Placeholder", _placeholder);


            //Los strings de los headers de las columnas, tomando el valor actual como base ES (esto es porque las columnas se crean dinámicamente al bindear la lista (nombres de parámetro crudo).
            foreach (DataGridViewColumn column in mainDGV.Columns)
            {
                _transMgr.AddString($"CustomDGVForm.DataGridView.Column.{column.Name}", column.HeaderText);
            }

            //AttachColumnSelector se encarga de registrar y traducir el Placeholder ("Seleccionar columna") inicial con su propia instancia de traductor.
            _searchBehavior.AttachColumnSelector(cbColumnsName);

        }

        //Método llamado por reflexión desde el Translation Manager cuando se notifica un cambio de idioma.
        public void NotifiedByTranslationManager() => ApplyTranslation();

        private void ApplyTranslation()
        {

            foreach (DataGridViewColumn col in mainDGV.Columns)
            {
                var translatedHeader = _transMgr.GetString($"CustomDGVForm.DataGridView.Column.{col.Name}");
                col.HeaderText = translatedHeader;
            }

            _searchBehavior.UpdatePlaceHolder(_transMgr.GetString("CustomDGVForm.TextBox.Placeholder")); //ASDFASDASFDFASD
            _searchBehavior.RefreshColumnsIfChanged();


            _transMgr.Apply();
        }





    }
}
