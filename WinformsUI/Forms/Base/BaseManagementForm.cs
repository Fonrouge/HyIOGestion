using BLL.DTOs;
using Shared;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;
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
        // DEPENDENCIAS INYECTADAS
        protected readonly IApplicationSettings _appSettings;
        protected readonly ITranslatableControlsManager _transMgr;
        protected readonly ICustomDGVFactory _dgvFactory;


        // CUSTOM CONTROLS 

        //--Ribbons + Relacionados--
        protected CustomDGVRibbon _dgvRibbonControls;
        protected EyeRestRibbon _eyeRestRibbonControls;
        protected ExportRibbon _exportRibbon; //Falta implementar

        protected Panel gigaRibbonContainer; //Contenedor Ribbon versión grande (con un TLP en autosize como contenedor macro, si se cambia la visibilidad del panel, automáticamente colapsa). Además, permite coloración uniforme al ser un bloque "sólido".
        protected Panel miniRibbonContainer; //Contenedor de Ribbon versión mini - Mismo principio que giga.
        protected TableLayoutPanel miniRibbonTLP; //TLP que va DENTRO del contenedor Giga. Otorga la cuadrícula sobre la que disponer los íconos.
        protected TextBox miniSearchBar; //Dirección que apunta a la SearchBar original de CustomDGVForm. Se secuestra la SearchBar de CustomDGVForm removiéndola de su contenedor y añadiéndola en un contenedor de esta misma clase. Evita problemas de doble suscripción de eventos y persiste en el estado que el usuario la dejó.
        protected Button btnToggleRibbon; //Botón que triggerea la misma acción que CTRL+H. Cambiar la visibilidad = !visibilidad de los contenedores mini y giga. Siempre distintos para simular colapso y extensión de uno y otro.
        protected bool searchBarIsKidnapped = false; //Flag para diferenciar en qué control está incrustado el TextBox SearchBar.


        //--DataGridView + Relacionados--
        protected CustomDGVForm _dgvForm; //Wrapper de DataGridView con funcionalidades añadidas.
        protected BindingList<TEntity> _entitiesList; //Lista que se usa como contenido del DGV de CustomDGVForm.
        protected TEntity _currentSelectedEntity; //Entidad reconstruida a partir de la fila seleccionada del DGV de CustomDGVForm.


        // FEEDBACK
        protected string _successOperationMessage; //Mensaje predeterminado para implementación en MessageBox o futuras implementaciones que no corten el flow de trabajo
        private Panel _feedbackBarContainer; //Contenedor donde se dispondrá el custom control que hará de "barrita pintada" (Feedback Bar).
        private SilentFeedbackBar _feedbackBar; //Lógica de pintado y animiación.
        Size _originalFeedbackBarSize = new Size(); //Tamaño original del contenedor de Feedback Bar.


        // EVENTOS ESTÁNDAR
        public event EventHandler CreateRequested;
        public event EventHandler<TEntity> UpdateRequested;
        public event EventHandler<TEntity> DeleteRequested;
        public event EventHandler ListAllRequested;
        public event EventHandler CloseRequested;
        public virtual event EventHandler OnceLoadedAdvice; //Virtual para que cada form hijo overrideé este evento indicando cuando terminó de cargar.

        // ATAJOS DE TECLADO
        private ShortcutManager _shortcutMgr;
        private byte _conditionalSearchBarStates = 0; //Mini máquina de estados para determinar qué hará el atajo condicional CTRL+B.

        public Guid ViewId { get; set; } = Guid.NewGuid(); //Al ser un formulario inyectado dentro de oro formulario, tiene que poder enviar un evento para que
                                                           //su contenedor padre sepa que está solicitando el cierre. Al enviar todos los formularios el mismo tipo de mensaje, si no hay un ID único se cerrarían todos los formularios en vez del solicitado.

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
        public virtual void ThemingNotifiedByConfigurationsModule()
        {

            try //No se busca catchear ningún error, simplemente evitar problemas en la Race-Condition de repintado del placeholder vs. el cambio global de tema.
            {
                SuspendLayout();
                DarkTheme.RedrawBorders = true;
                DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
            }
            catch { }
            finally
            {
                _dgvForm.RepaintSearchBarPlaceHolder(DarkTheme.GetCurrentPalette().TextSecondary);
                ResumeLayout();
            }

        }


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
       .Add("Ctrl+H", () => ToolStripsPanelToggle(gigaRibbonContainer))
       .Add("Ctrl+F", () => _dgvForm.ToggleFiltersPanel())
       .BindWheelZoom(() => _dgvForm.ZoomIn(), () => _dgvForm.ZoomOut()) // Ctrl + rueda
       ;




        /// <summary>
        /// Recibe un botón de una clase hija para colapsar/restaurar una de las dos barras Ribbon. La "mini" o la "giga".
        /// </summary>
        /// <param name="btn"></param>
        public void AttachBtnToggleRibbon(Button btn)
        {
            btnToggleRibbon = btn;
            btnToggleRibbon.Click += BtnToggleRibbon_Click;
        }


        private void BtnToggleRibbon_Click(object sender, EventArgs e) => ToolStripsPanelToggle(gigaRibbonContainer);


        /// <summary>
        /// Colapsa una Ribbon y restaura su versión opuesta. Si expande la pequeña, contrae la grande o viceversa. Además pide prestada la SearchBae a CustomDGVForm o la devuelve para moverla entre contenedores.
        /// </summary>
        /// <param name="tsPanel"></param>
        public void ToolStripsPanelToggle(Panel tsPanel = null)
        {
            if (tsPanel == null) return;

            try
            {
                SuspendLayout();

                gigaRibbonContainer = tsPanel;
                gigaRibbonContainer.Visible = !gigaRibbonContainer.Visible;

                if (miniRibbonContainer != null)
                    miniRibbonContainer.Visible = !gigaRibbonContainer.Visible;

                if (gigaRibbonContainer.Visible == true)
                {
                    if (miniRibbonContainer != null)
                    {
                        miniRibbonContainer.Visible = false;
                        _dgvForm.ReturnSearchBar(miniSearchBar);
                        searchBarIsKidnapped = false;
                        _dgvForm.SwitchHorizontalDividerVisibility(true);
                    }
                }
                else
                {
                    if (miniRibbonContainer != null)
                    {
                        miniRibbonContainer.Visible = true;
                        miniSearchBar = _dgvForm.AskForSearchBar();
                        miniRibbonTLP.Controls.Add(miniSearchBar, 1, 0);
                        searchBarIsKidnapped = true;
                        _dgvForm.SwitchHorizontalDividerVisibility(false);
                    }
                }
            }
            catch { }
            finally { ResumeLayout(); }


        }
      protected override CreateParams CreateParams
      {
          get
          {
              CreateParams cp = base.CreateParams;
              // 0x02000000 = WS_EX_COMPOSITED
              cp.ExStyle |= 0x02000000;
              return cp;
          }
      }
   
        /// <summary>
        /// Mini máquina de estados que controla el comportamiento edl atajo de teclado CTRL+B. Dependiendo donde esté el foco y cómo estén colapsadas o no las Ribbon, tendrá diferentes comportamientos. Se detalla su comportamiento
        /// en el propio código. Ver método.
        /// </summary>
        public void ConditionalSearchBarBehavior()
        {

            SuspendLayout();

            if (_dgvForm.IsToggleSearchBarVisible() && _conditionalSearchBarStates < 1) //Si está visible y nada pasó
            {
                if (!searchBarIsKidnapped)
                {
                    _dgvForm.FocusSearchBar();
                    _conditionalSearchBarStates++;
                }
            }

            else if (!_dgvForm.IsToggleSearchBarVisible() && _conditionalSearchBarStates < 1) //Si está visible y nada pasó
            {
                miniSearchBar.Focus();
            }

            else if (_conditionalSearchBarStates != 0) //Si está visible y con foco, colapsa
            {
                if (!searchBarIsKidnapped)
                {
                    _dgvForm.ToggleSearchBar();
                    _conditionalSearchBarStates = 0;
                }
            }

            else if (_conditionalSearchBarStates == 0) //Si está colapsada, se activa y hace foco
            {
                if (!searchBarIsKidnapped)
                {
                    _dgvForm.ToggleSearchBar();
                    _dgvForm.FocusSearchBar();
                }
            }

            ResumeLayout();
        }



        /// <summary>
        /// Método para que clases hijas puedan "plug & play" un contenedor -Panel- para obtener la lógica de pintado y animado de una Silent Feedback Bar.
        /// </summary>
        /// <param name="feedbackBarPanel"></param>
        protected void AttachFeedbackBarContainer(Panel feedbackBarPanel)
        {
            _feedbackBarContainer = feedbackBarPanel;
            _originalFeedbackBarSize = _feedbackBarContainer.Size;

            _feedbackBar = new SilentFeedbackBar();
            _feedbackBarContainer.Controls.Add(_feedbackBar);
            _feedbackBar.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Método para disparar una vez el formulario activo pierda el foco. La idea es tener un feedback visual claro de qué formulario tiene el foco en qué momento.
        /// </summary>
        protected void ChangeActivationStateFeedbackBar() => _feedbackBar.IsActiveModule = !_feedbackBar.IsActiveModule;


        /// <summary>
        /// Método para cambiar el estado desde el Presenter según sea necesario (Cargando > Success en try/Error en catch). Se manejan automáticamente los tiempos de
        /// notificación visual.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task SetFeedbackState(FeedbackState state)
        {
            if (this.InvokeRequired)
            {
                await (Task)this.Invoke(new Func<Task>(() => SetFeedbackState(state)));
                return;
            }

            try
            {
                _feedbackBarContainer.Size = new Size(_originalFeedbackBarSize.Width, _originalFeedbackBarSize.Height + 5);
                await _feedbackBar.TriggerFeedbackAsync(state);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Feedback: {ex.Message}");
            }
            finally
            {               
                _feedbackBarContainer.Size = _originalFeedbackBarSize;
                await _feedbackBar.TriggerFeedbackAsync(FeedbackState.Idle);
            }
        }
        public void IdleFeedbackPanel(object sender, EventArgs e) => SetFeedbackState(FeedbackState.Idle);



        //====================================================
        //          CICLO DE VIDA Y CONFIGURACIÓN
        //====================================================
        protected override void OnLoad(EventArgs e)
        {
            DoubleBuffering.TryForAllControls(this.Controls);
            SuspendLayout();

            base.OnLoad(e);

            if (!DesignMode)
            {
                SetBaseFormAppearance();
                ApplyTranslation();
            }
            ResumeLayout();
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

        //------------------------------REVISAR NUEVO SISTEMA DE FEEDBACK SILENCIOSO ----------------------------------------------
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
        protected void OnCreateRequest(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CreateRequested?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        protected void OnUpdateRequest(object sender, EventArgs e)
        {
            _dgvForm.EnsureDgvRowSelection();

            if (_currentSelectedEntity != null)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    UpdateRequested?.Invoke(this, _currentSelectedEntity);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }

            }

        }

        protected void OnDeleteRequest(object sender, EventArgs e)
        {
            _dgvForm.EnsureDgvRowSelection();

            if (_currentSelectedEntity != null)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    DeleteRequested?.Invoke(this, _currentSelectedEntity);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }

            }
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