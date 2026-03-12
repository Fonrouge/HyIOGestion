using BLL.DTOs;
using Shared;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls;
using WinformsUI.UserControls.CustomDGV;

namespace WinformsUI.Forms.Base
{
    /// <summary>
    /// Formulario base para la gestión (CRUD) de entidades (Create va a parte, el form se encarga de los casos restantes C(RUDL))
    /// Encapsula la lógica común de Grillas, Temas, Traducciones y Eventos estándar.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de DTO que gestiona este formulario (ej. ClientDTO).</typeparam>
    public class BaseManagementForm<TEntity> : Form, IView where TEntity : IDto
    {
        //Dependencias Protegidas (Accesibles por los hijos)
        protected readonly IApplicationSettings _appSettings;
        protected readonly ITranslatableControlsManager _transMgr;
        protected readonly ICustomDGVFactory _dgvFactory;

        protected CustomDGVForm _dgvForm;
        protected BindingList<TEntity> _entitiesList;
        protected TEntity _currentSelectedEntity;

        // Mensaje de éxito genérico (puede ser sobreescrito o usado tal cual)
        protected string _successOperationMessage;

        // EventHandler genéricos para estandarizar, se espera que cada form suscriba sus propios eventos a estos handlers (ej: public event EventHandler CreateClientRequested{add => CreateRequested += value; remove => CreateRequested -= value;}
        public event EventHandler CreateRequested;
        public event EventHandler<TEntity> UpdateRequested;
        public event EventHandler<TEntity> DeleteRequested;
        public event EventHandler ListAllRequested;


        #region Constructor Base
        public BaseManagementForm() { }

        public BaseManagementForm
        (
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr,
            ICustomDGVFactory dgvFact)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _transMgr = transMgr ?? throw new ArgumentNullException(nameof(transMgr));
            _dgvFactory = dgvFact ?? throw new ArgumentNullException(nameof(dgvFact));

            _successOperationMessage = _appSettings.SuccessOnOperation;
            _entitiesList = new BindingList<TEntity>();

            this.FormClosed += (s, e) =>
            {
           //     _transMgr.CleanupForm(this);
            };
        }
        public void ApplyGlobalPalette() { }


        #endregion

        #region Ciclo de Vida y Configuración (Template Methods)

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DoubleBuffering.TryForAllControls(this.Controls);

            if (!DesignMode)
            {
                SetBaseFormAppearance();
                ApplyTranslation(); // Llama al método abstracto/virtual
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!DesignMode)
            {
                OnListAllRequest(); // Carga inicial de datos automática
            }
        }

        private void SetBaseFormAppearance()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;
        }

        #endregion



        #region Métodos Públicos (Contrato IView Genérico)

        public void CachingList(IEnumerable<TEntity> list)
        {
            _entitiesList = list.ToBindingList();
        }

        public void FillDGV()
        {
            if (_dgvForm != null)
            {
                _dgvForm.FillDGV<TEntity>(_entitiesList);
            }
        }

        public void ShowOperationResult(OperationResult<TEntity> opRes, Func<TEntity, string> successMessageSelector = null)
        {
            if (!opRes.Errors.Any())
            {
                string msg = successMessageSelector != null
                    ? _successOperationMessage + " " + successMessageSelector(_currentSelectedEntity)
                    : _successOperationMessage;

                MessageBox.Show(msg); // Reemplazar por IMessageDialogService inyectado cuando refactorice
            }
            else
            {
                foreach (var error in opRes.Errors)
                {
                    MessageBox.Show($"{error.Message}\n{error.RecommendedAction}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        public void NotifiedByTranslationManager() => ApplyTranslation(); //Notified by reflection on TranslatorManager class. That's why it has 0 references.

        public void ApplyTranslation() => _transMgr.Apply();

        #endregion



        #region Métodos Protegidos para los Hijos (Hooks)

        /// <summary>
        /// Método opcional que se ejecuta cuando cambia la selección en la grilla.
        /// </summary>
        protected virtual void OnEntitySelected(TEntity entity) { }

        /// <summary>
        /// Inicializa y configura el DataGridView dentro del panel contenedor proporcionado.
        /// </summary>
        /// <param name="containerPanel">El Panel donde se incrustará la grilla.</param>
        protected void InitializeDGV(Control containerPanel)
        {
            if (containerPanel == null) throw new ArgumentNullException(nameof(containerPanel));

            _dgvForm = _dgvFactory.Create(_transMgr);
            _dgvForm.TopLevel = false;
            _dgvForm.Dock = DockStyle.Fill;

            containerPanel.Controls.Add((Form)_dgvForm);
            _dgvForm.Show();

            // Cableado automático de selección
            _dgvForm.SelectedRowChanged += (s, entity) =>
            {
                _dgvForm.EnsureDgvRowSelection();

                if (entity != null && entity is TEntity typedEntity)
                {
                    _currentSelectedEntity = typedEntity;
                    OnEntitySelected(typedEntity); // Hook para hijos
                }
            };
        }
        #endregion


        #region Disparadores de Eventos (Event Invokers)

        protected void OnCreateRequest() => CreateRequested?.Invoke(this, EventArgs.Empty);

        protected void OnUpdateRequest()
        {
            _dgvForm.EnsureDgvRowSelection();

            if (_currentSelectedEntity != null)
                UpdateRequested?.Invoke(this, _currentSelectedEntity);
        }

        protected void OnDeleteRequest()
        {
            _dgvForm.EnsureDgvRowSelection();

            if (_currentSelectedEntity != null)
                DeleteRequested?.Invoke(this, _currentSelectedEntity);
        }

        protected void OnListAllRequest()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ListAllRequested?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "BaseManagementForm";
            this.ResumeLayout(false);

        }
    }
}