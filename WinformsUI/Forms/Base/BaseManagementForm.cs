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
    /// Formulario base para la gestión (CRUD) de entidades.
    /// Encapsula la lógica común de Grillas, Temas, Traducciones, Cierre y Eventos estándar.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de DTO que gestiona este formulario.</typeparam>
    public class BaseManagementForm<TEntity> : Form, IView where TEntity : IDto
    {
        protected readonly IApplicationSettings _appSettings;
        protected readonly ITranslatableControlsManager _transMgr;
        protected readonly ICustomDGVFactory _dgvFactory;

        protected CustomDGVFunctions _dgvControls;
        protected CustomDGVForm _dgvForm;
        protected BindingList<TEntity> _entitiesList;
        protected TEntity _currentSelectedEntity;

        protected string _successOperationMessage;

        // Eventos estándar
        public event EventHandler CreateRequested;
        public event EventHandler<TEntity> UpdateRequested;
        public event EventHandler<TEntity> DeleteRequested;
        public event EventHandler ListAllRequested;
        public event EventHandler CloseRequested;



        public BaseManagementForm() { }

        public BaseManagementForm
        (
            IApplicationSettings appSettings,
            ITranslatableControlsManager transMgr,
            ICustomDGVFactory dgvFact
        )
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _transMgr = transMgr ?? throw new ArgumentNullException(nameof(transMgr));
            _dgvFactory = dgvFact ?? throw new ArgumentNullException(nameof(dgvFact));

            _successOperationMessage = _appSettings.SuccessOnOperation;
            _entitiesList = new BindingList<TEntity>();

            // Suscripción automática al cierre para notificar al Presenter
            this.FormClosed += HandleBaseFormClosed;



        }

        public void ApplyGlobalPalette() { }

     
        //====================================================
        //          CICLO DE VIDA Y CONFIGURACIÓN
        //====================================================
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffering.TryForAllControls(this.Controls);

            if (!DesignMode)
            {
                SetBaseFormAppearance();
                ApplyTranslation();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!DesignMode)
            {
                OnListAllRequest(null, e);
            }
        }

        private void SetBaseFormAppearance()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;
        }

        private void HandleBaseFormClosed(object sender, FormClosedEventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }


        //====================================================
        //             MÉTODOS PÚBLICOS (IView)
        //====================================================
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

        protected void InitializeDGVControls() => _dgvControls.TargetDGV = _dgvForm;

        public void ShowOperationResult(OperationResult<TEntity> opRes)
        {
            if (opRes.Success)
            {
                MessageBox.Show(_successOperationMessage, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                foreach (var error in opRes.Errors)
                {
                    MessageBox.Show($"{error.Message}\n{error.RecommendedAction}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public virtual void CloseView()
        {
            if (!this.IsDisposed)
            {
                this.Close();
            }
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();

        public void ApplyTranslation() => _transMgr.Apply();



        //====================================================
        //             MÉTODOS PROTEGIDOS (HOOKS)
        //====================================================
        protected virtual void OnEntitySelected(TEntity entity) { }

        protected void InitializeDGV(Control containerPanel)
        {
            if (containerPanel == null) throw new ArgumentNullException(nameof(containerPanel));

            _dgvForm = _dgvFactory.Create(_transMgr);
            _dgvForm.TopLevel = false;
            _dgvForm.Dock = DockStyle.Fill;

            containerPanel.Controls.Add((Form)_dgvForm);
            _dgvForm.Show();

            _dgvForm.SelectedRowChanged += (s, entity) =>
            {
                _dgvForm.EnsureDgvRowSelection();

                if (entity != null && entity is TEntity typedEntity)
                {
                    _currentSelectedEntity = typedEntity;
                    OnEntitySelected(typedEntity);
                }
            };
        }


        //====================================================
        //              DISPARADORES DE EVENTOS
        //====================================================
        protected void OnCreateRequest(object sender, EventArgs e) => CreateRequested?.Invoke(this, EventArgs.Empty);

        protected void OnUpdateRequest(object sender, EventArgs e)
        {
            _dgvForm.EnsureDgvRowSelection();
            if (_currentSelectedEntity != null)
                UpdateRequested?.Invoke(this, _currentSelectedEntity);
        }

        protected void OnDeleteRequest(object sender, EventArgs e)
        {
            _dgvForm.EnsureDgvRowSelection();
            if (_currentSelectedEntity != null)
                DeleteRequested?.Invoke(this, _currentSelectedEntity);
        }

        protected void OnListAllRequest(object sender, EventArgs e)
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


        //====================================================
        //                   LIMPIEZA
        //====================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.FormClosed -= HandleBaseFormClosed;
                _entitiesList = null;
                _dgvForm = null;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseManagementForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "BaseManagementForm";
            this.ResumeLayout(false);

        }
    }
}