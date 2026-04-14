using BLL.DTOs;
using Shared;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using WinformsUI.Infrastructure.Shortcuts;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.UserControls;
using WinformsUI.UserControls.CustomDGV;
using WinformsUI.UserControls.Ribbon;

namespace WinformsUI.Forms.Base
{
    /// <summary>
    /// Formulario base para la gestión (CRUD) de entidades.
    /// Encapsula la lógica común de Grillas, Temas, Traducciones, Cierre y Eventos estándar.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de DTO que gestiona este formulario.</typeparam>
    public class BaseManagementForm<TEntity> : Form, IView where TEntity : IDto
    {
        // Dependencias inyectadas
        protected readonly IApplicationSettings _appSettings;
        protected readonly ITranslatableControlsManager _transMgr;
        protected readonly ICustomDGVFactory _dgvFactory;

        // Controles comunes 
        protected CustomDGVRibbon _dgvRibbonControls;
        protected EyeRestRibbon _eyeRestRibbonControls;
        protected CustomDGVForm _dgvForm;
        protected BindingList<TEntity> _entitiesList;
        protected TEntity _currentSelectedEntity;

        // Mensaje estándar para operaciones 
        protected string _successOperationMessage;

        // Eventos estándar
        public event EventHandler CreateRequested;
        public event EventHandler<TEntity> UpdateRequested;
        public event EventHandler<TEntity> DeleteRequested;
        public event EventHandler ListAllRequested;
        public event EventHandler CloseRequested;
        public virtual event EventHandler OnceLoadedAdvice;

        // Atajos de teclado
        private ShortcutManager _shortcutMgr;
        public Guid ViewId { get; set; } = Guid.NewGuid();

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

            SetShortcuts();
        }

        //====================================================
        //                  ESTÉTICA GENERAL
        //====================================================
        public virtual void ThemingNotifiedByConfigurationsModule() { }


        //====================================================
        //                     SHORTCUTS
        //====================================================
        private void SetShortcuts() => _shortcutMgr = ShortcutManager.Attach(this)

       .Add("Ctrl+B", () => ConditionalSearchBarBehavior()) //B - (B)uscar cliente
       .Add("Ctrl+A", () => OnCreateRequest(null, null)) //A - (A)gregar cliente
       .Add("Ctrl+E", () => OnDeleteRequest(null, null)) //E - (E)liminar cliente
       .Add("Ctrl+M", () => OnUpdateRequest(null, null)) //M - (M)odificar cliente
       .Add("Ctrl+R", () => OnListAllRequest(null, null)) //R - (R)efrescar lista de clientes

       .Add("Ctrl+N", () => OnCreateRequest(null, null)) //Ctrl+N - Agregar cliente (congruencia con estándar de Windows)
       .Add("Delete", () => OnDeleteRequest(null, null)) //Del - Eliminar cliente (congruencia con estándar de Windows)
       .Add("F2", () => OnUpdateRequest(null, null)) //F2 - Modificar cliente (congruencia con estándar de Windows)
       .Add("F5", () => OnListAllRequest(null, null)) //F5 - Refrescar lista de clientes (congruencia con estándar de Windows)

       .Add("Alt+right", () => _dgvForm.NudgeColumnWidth(16)) //Dirección natural para "alargar" columna
       .Add("Alt+left", () => _dgvForm.NudgeColumnWidth(-16)) //Dirección contraria a "alargar" = contraer
       .Add("Alt+up", () => _dgvForm.NudgeRowHeight(4)) //Dirección natural para aumentar altura
       .Add("Alt+down", () => _dgvForm.NudgeRowHeight(-4)) //Dirección natural para disminuiraltura
       .Add("Alt+shift+up", () => _dgvForm.AutoFitRowAndHeader()) //Pudiendo tomarse como referencia arriba o abajo, se elige arriba para indicar altura en "positivo"
       .Add("Alt+shift+right", () => _dgvForm.AutoFitColumns()) //Misma lógica que para filas pero con derecha como referencia para indicar "positivo" en ancho de columnas
       .Add("Ctrl+W+", () => HandleBaseFormClosed(this, null)) //Al ser un form inyectado dentro de otro (HostForm en esta implementación) debe poder notificarlo "hacia afuera", por tanto se termina enviando un pedido al Messenger.
       .Add("Ctrl+plus", () => _dgvForm.ZoomIn())
       .Add("Ctrl+minus", () => _dgvForm.ZoomOut())
       .Add("Ctrl+H", () => ToolStripsPanelToggle(toolStripsPanel))
       .Add("Ctrl+F", () => _dgvForm.ToggleFiltersPanel())
       .BindWheelZoom(() => _dgvForm.ZoomIn(), () => _dgvForm.ZoomOut()) // Ctrl + rueda
       ;

        private byte _conditionalSearchBarStates = 0;
        private Panel toolStripsPanel;
        public void ToolStripsPanelToggle(Panel tsPanel = null)
        {
            if (tsPanel == null) return;
            toolStripsPanel = tsPanel;

            toolStripsPanel.Visible = !toolStripsPanel.Visible;
        }
        public void ConditionalSearchBarBehavior()
        {
            if (_dgvForm.IsToggleSearchBarVisible() && _conditionalSearchBarStates < 1)
            {
                _dgvForm.FocusSearchBar();
                _conditionalSearchBarStates++;
            }
            else if (_conditionalSearchBarStates != 0)
            {
                _dgvForm.ToggleSearchBar();
                _conditionalSearchBarStates = 0;
            }
            else if (_conditionalSearchBarStates == 0)
            {
                _dgvForm.ToggleSearchBar();
                _dgvForm.FocusSearchBar();
            }
        }


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

        protected void InitializeRibbonControls()
        {
            _dgvRibbonControls.TargetDGV = _dgvForm;
            _eyeRestRibbonControls.TargetForm = this;
        }

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

        public void SelectFirstGridRow() => _dgvForm.EnsureDgvRowSelection();
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
        protected void HandleBaseFormClosed(object sender, FormClosedEventArgs e)
        {
            CloseRequested?.Invoke(this.ViewId, EventArgs.Empty);
        }


        //====================================================
        //                   LIMPIEZA
        //====================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
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