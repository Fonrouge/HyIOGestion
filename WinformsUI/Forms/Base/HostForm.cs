using Presenter.Messaging;
using Shared.ArchitecturalMarkers;
using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Infrastructure;
using WinformsUI.Infrastructure.Translations;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Forms.Base
{
    public partial class HostForm : Form, IHostFormActions
    {
        #region === Campos y Propiedades de Configuración ===

        protected Panel _parentContainer;
        private FlowLayoutPanel _upperMenuPanel;
        private IAppEnvironment _appEnv { get; set; }
        private readonly ITranslatableControlsManager _transMgr;
        private readonly IMessenger _messenger;

        // Variables para control de estado
        private Size _previousSize;
        private Point _previousLocation;
        public bool IsExpanded { get; set; } = false;
        public bool IsMinimized { get; set; } = false;
        public bool IsMaximized { get; set; } = false;

        private string _title;


        #region === Inicialización ===

        public HostForm
        (
            ITranslatableControlsManager transMgr,
            IMessenger messenger
        )
        {
            _transMgr = transMgr;
            _messenger = messenger;

            InitializeComponent();
        }


        public void SetTitle(string Title)
        {
            _title = Title;
            lblTitle.Text = Title;
            this.Text = _title;

            if (_minimizedWindowBtn != null)
                _minimizedWindowBtn.Text = Title;
        }

        public string GetTitle()
        {
            return _title;
        }


        public void ApplyTranslation() => _transMgr.Apply();

        //Variables estéticas
        protected Palette InternalPalette { get; set; } = DarkTheme.GetCurrentPalette();

        // Variables para arrastre y redimensionado
        private bool _isPosibleDrag = true;
        private const int _resizeBorder = 10;


        #region === Constantes Windows API + Drag===

        private const int WM_NCHITTEST = 0x84;
        private const int WM_NCLBUTTONDOWN = 0xA1;

        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        // Redimensionamiento
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int WM_NCLBUTTONDBLCLK = 0xA3;

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();


        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);

        private Point _dragStartLocation;
        private Point _dragStartFormRelative;
        #endregion

        #region === Campos de Minimizacion Personalizada ===
        private Image _rightMenuButtonIcon;
        private Button _minimizedWindowBtn;

        private FormTypeEnum _formType { get; set; }




        #endregion




        public void SetContent(object content)
        {
            // 1. Verificamos si el contenido es un Control de WinForms
            // (Un Form también hereda de Control, así que esto es seguro)
            if (content is Control uiControl)
            {
                // Si es un Form, aplicamos configuraciones de nivel superior
                if (uiControl is Form formContent)
                {
                    formContent.TopLevel = false;
                }

                // 2. Limpiamos el contenedor por si ya tenía algo (evita solapamientos)
                internalContainer.Controls.Clear();

                // 3. Inyectamos el contenido en el "hueco" del HostForm
                uiControl.Dock = DockStyle.Fill;
                uiControl.Visible = true; // Nos aseguramos de que sea visible

                internalContainer.Controls.Add(uiControl);
                uiControl.BringToFront();

                // 4. Si el contenido es un Form, lo mostramos explícitamente
                if (uiControl is Form f) f.Show();
            }
            else
            {
                throw new ArgumentException("El contenido proporcionado no es un control válido para WinForms.");
            }


        }

        public void Initialize(IAppEnvironment appEnv)
        {
            // 1. Validación y asignación del contrato agnóstico - Se guarda la referencia como interfaz IAppEnvironment
            this._appEnv = appEnv ?? throw new ArgumentNullException(nameof(appEnv));

            // 2. Configuración de infraestructura - Como HostForm está en la capa UI, aquí es donde se procesa el entorno
            ConfigureEnvironment(appEnv);

            // 3. Estética y eventos
            UIAesthetics();
            SetUpBasicEvents();


            deudaTecnicaVerPorQuéBotonesNoPintanHover();
        }

        private void UIAesthetics()
        {
            EnableDoubleBuffering(this);
            this.FormBorderStyle = FormBorderStyle.None;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            MaximumSize = Screen.FromControl(this).WorkingArea.Size;


            switch (_formType)
            {
                case FormTypeEnum.Main:

                    throw new NotImplementedException("Not implemented yet");

                //Formularios internos.
                case FormTypeEnum.Module:

                    DarkTheme.RedrawBorders = true;
                    break;

                case FormTypeEnum.Configs:
                    MaximizeBox = false;
                    WindowState = FormWindowState.Normal;

                    break;

                default: throw new ArgumentException("Not supporter Form type");


            }

            this.Padding = new Padding(3, 0, 3, 3);
            ApplyPalette(InternalPalette);
        }


        private void SetUpBasicEvents()
        {
            WireTitleBarEvents();

            CreateInstanceMinimizedWindow(); //Ya se deja instanciada su representación minimizada antes de asociarla a una acción que requiera su existencia en los eventos de botones.
            WireButtonsEvents();
        }


        private readonly Guid _id = Guid.NewGuid();  // Nuevo: ID único
        public Guid Id => _id;  // Nuevo: Exposición


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_minimizedWindowBtn != null)
            {
                if (_parentContainer != null && _upperMenuPanel.Controls.Contains(_minimizedWindowBtn))
                {
                    _upperMenuPanel.Controls.Remove(_minimizedWindowBtn);
                }
                _minimizedWindowBtn.Dispose();
            }
            if (_parentContainer != null)
            {
                _parentContainer.SizeChanged -= ContainerSizeChanged;
            }
            // Notificación vía Messenger: Usa GUID como ID (robusto, no depende de _title)
            var closedMessage = new HostFormClosedMessage(_id, this);
            _messenger.Send(closedMessage);
            if (e != null)
                base.OnFormClosing(e);
        }
        #endregion

        #endregion



        public event EventHandler ContractRequested;
        public event EventHandler ExpandRequested;
        public event EventHandler CloseWindowRequested;

        public event EventHandler RestoreWindowFromMinimizedRequested;
        public event EventHandler MinimizeWindowRequested;





        public void WireButtonsEvents()
        {
            btnClose.Click += (s, e) => CloseWindowRequested?.Invoke(this, EventArgs.Empty);

            btnExpand.Click += (s, e) =>
            {
                SuspendLayout();
                if (IsExpanded) ContractRequested?.Invoke(this, EventArgs.Empty);
                else if (!IsExpanded) ExpandRequested?.Invoke(this, EventArgs.Empty);
                ResumeLayout();
            };

            btnMinimize.Click += (s, e) =>
            {
                SuspendLayout();
                MinimizeWindowRequested?.Invoke(this, EventArgs.Empty);
                ResumeLayout();
            };

            _minimizedWindowBtn.Click += (s, e) =>
            {
                SuspendLayout();
                RestoreWindowFromMinimizedRequested?.Invoke(this, EventArgs.Empty);
                ResumeLayout();
            };
        }


        private void ApplyPalette(Palette p)
        {
            InternalPalette = p;
            DarkTheme.Apply(this, InternalPalette);
        }

        public void ConfigureEnvironment(IAppEnvironment appEnv)
        {
            // 1. Intentamos tratar el entorno como la implementación de WinForms
            if (appEnv is AppEnvironment winEnv)
            {
                // 2. Extraemos propiedades visuales y de estado
                this.InternalPalette = winEnv.InternalPalette;
                this._formType = winEnv.FormType;

                // 3. Si es el formulario principal, no necesita lógica de host
                if (_formType == FormTypeEnum.Main) return;

                // 4. Configuración de Contenedores
                // Como estamos en HostForm, ya sabemos que estos objetos son Controles
                if (winEnv.DashboardContainer != null)
                    SetDashboardControl(winEnv.DashboardContainer);

                if (winEnv.RightMenu != null)
                    SetRightMenuControl(winEnv.RightMenu);

                // 5. Configuración de Iconografía
                if (winEnv.ModuleIcon != null)
                    _rightMenuButtonIcon = winEnv.ModuleIcon;
            }
            else
            {
                // Opcional: Manejo de error si se intenta inicializar con un entorno incompatible
                throw new InvalidOperationException("El entorno proporcionado no es compatible con la infraestructura de WinForms.");
            }
        }

        private void CreateInstanceMinimizedWindow()
        {
            _minimizedWindowBtn = new Button()
            {
                Size = new Size(_title.Length + 150, 40),
                AutoSize = false,
                FlatStyle = FlatStyle.Flat
            };

            DarkTheme.ApplyGradientBackground
            (
                c: _minimizedWindowBtn,
                begin: Darken(InternalPalette.Accent, 0.8),
                end: Color.Black
            );

            _minimizedWindowBtn.FlatAppearance.BorderSize = 2;
            _minimizedWindowBtn.FlatAppearance.BorderColor = Darken(InternalPalette.Accent, 0.6); //DarkTheme.Darken(InternalPalette.Accent, 0.5)

            _minimizedWindowBtn.Text = _title;
            _minimizedWindowBtn.ForeColor = Color.White;
            _minimizedWindowBtn.Image = _rightMenuButtonIcon ?? null;
            _minimizedWindowBtn.TextImageRelation = TextImageRelation.ImageBeforeText;

            _minimizedWindowBtn.Padding = new Padding(10, 10, 10, 10);
            _minimizedWindowBtn.Margin = new Padding(0, 0, 0, 0);
            _minimizedWindowBtn.Font = new Font("Microsoft YaHei UI", 9f, FontStyle.Italic);
        }



        public void ContractWindow()
        {
            if (_upperMenuPanel == null) return;
            if (!IsExpanded) return;

            SuspendLayout();
            this.Size = _previousSize;
            this.Location = _previousLocation;
            IsExpanded = false;
            ResumeLayout();
        }

        public void ExpandWindow()
        {
            if (_upperMenuPanel == null) return;
            if (IsExpanded) return;

            SuspendLayout();
            _previousSize = this.Size;
            _previousLocation = this.Location;

            this.Location = new Point(0, 0);
            this.Size = _parentContainer.Size;
            IsExpanded = true;
            ResumeLayout();
        }

        public void CloseWindow()
        {
            OnFormClosing(null);
            this.Close();
        }

        public void RestoreWindowFromMinimized()
        {
            if (_upperMenuPanel == null) return;
            if (!IsMinimized) return;

            SuspendLayout();

            _upperMenuPanel.Controls.Remove(_minimizedWindowBtn);
            this.Visible = true;
            IsMinimized = false;

            ResumeLayout();
        }

        public void MinimizeWindow()
        {
            if (IsMinimized) return;

            SuspendLayout();

            try //DEUDA TÉCNICA IMPORTANTE - BYPASSEO UN BUG (con el try-catch) EN EL CUAL SE INTENTA MINIMIZAR UN OBJETO DESECHADO Y EL PROGRAMA COLAPSA. ATENDER ---IMPORTANTE--- A EVITAR FUGAS DE MEMORIA POSIBLEMENTE PROVENINENTES DEL PRESENTER DE HOSTFORM
            {
                _upperMenuPanel.Controls.Add(_minimizedWindowBtn);
                this.Visible = false;
                IsMinimized = true;
            }
            catch { }

            ResumeLayout();
        }



        private void SetRightMenuControl(FlowLayoutPanel flp) => _upperMenuPanel = flp ?? throw new ArgumentNullException($"{nameof(flp)} cannot be null");


        public void WireTitleBarEvents()
        {
            // 1. Asignamos la lógica al PANEL (Header)
            if (tlpTitleBar != null)
            {
                this.tlpTitleBar.MouseDown += Common_MouseDown;
                this.tlpTitleBar.MouseMove += Common_MouseMove;
                this.tlpTitleBar.MouseUp += Common_MouseUp;
                this.tlpTitleBar.MouseDoubleClick += Common_MouseDoubleClick;
            }
            else
            {
                this.MouseDown += Common_MouseDown;
                this.MouseMove += Common_MouseMove;
                this.MouseUp += Common_MouseUp;
                this.MouseDoubleClick += Common_MouseDoubleClick;
            }

            // 2. Asignamos la misma lógica al LABEL (Título)
            // Esto hace que el label sea "transparente" al arrastre: funciona igual que el panel.
            if (lblTitle != null)
            {
                lblTitle.MouseDown += Common_MouseDown;
                lblTitle.MouseMove += Common_MouseMove;
                lblTitle.MouseUp += Common_MouseUp;
                lblTitle.MouseDoubleClick += Common_MouseDoubleClick;
            }

            // Eventos de estado (Foco, etc.)
            this.Activated += OnFormActiveStateChanged;
            this.Deactivate += OnFormActiveStateChanged;
            this.Enter += OnFormActiveStateChanged;
            this.Leave += OnFormActiveStateChanged;
            this.GotFocus += OnFormActiveStateChanged;

        }

        public void EnableDoubleBuffering(Control control)
        {
            // Esto habilita la magia interna de Windows para que pinte en memoria antes de mostrarlo
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty |
                BindingFlags.Instance |
                BindingFlags.NonPublic,
                null, control, new object[] { true });
        }


        #region === Lógica de Arrastre y Doble Click ===

        private void Common_MouseDown(object sender, MouseEventArgs e)
        {
            this.BringToFront();

            if (e.Button == MouseButtons.Left)
            {
                _isPosibleDrag = true;

                _dragStartLocation = e.Location;
                _dragStartFormRelative = this.PointToClient(Cursor.Position);
            }
        }

        private void Common_MouseMove(object sender, MouseEventArgs e)
        {


            if (_isPosibleDrag && e.Button == MouseButtons.Left)
            {
                // Usamos la posición actual en pantalla
                Point currentScreenPos = Cursor.Position;

                // Calculamos delta total desde el inicio del click
                int deltaX = currentScreenPos.X - _dragStartFormRelative.X; // _dragStartFormRelative debe ser PointToScreen al inicio
                                                                            // Nota: Tu lógica original usaba e.Location local, lo cual al mover el form causa feedback loop si no se maneja bien.

                // Mejor enfoque: Calcular distancia desde el punto de click original en PANTALLA
                int distMoved = (int)Math.Sqrt(Math.Pow(currentScreenPos.X - _dragStartLocation.X, 2) + Math.Pow(currentScreenPos.Y - _dragStartLocation.Y, 2));

                if (distMoved > 4) // Umbral
                {
                    // A. LÓGICA DE MAXIMIZADO (Despegue)
                    bool isMaximized = (_parentContainer == null && this.WindowState == FormWindowState.Maximized)
                                       || (_parentContainer != null && IsExpanded);

                    if (isMaximized)
                    {
                        // ... Tu lógica de cálculo de Ratio (es excelente, no la cambies) ...

                        // IMPORTANTE: Al restaurar, actualiza _dragStartLocation a la posición actual del mouse
                        // para que el arrastre continúe suavemente desde ahí sin saltos.
                        _dragStartLocation = currentScreenPos;

                        // Ajustamos el punto de agarre relativo al nuevo tamaño del form
                        _dragStartFormRelative = this.PointToClient(currentScreenPos);
                    }

                    // B. ARRASTRE
                    // Calculamos la nueva posición de la esquina superior izquierda del form
                    Point newPos = new Point(currentScreenPos.X - _dragStartFormRelative.X, currentScreenPos.Y - _dragStartFormRelative.Y);

                    // Conversión de coordenadas si estamos en un panel
                    if (this.Parent != null)
                    {
                        newPos = this.Parent.PointToClient(currentScreenPos);
                        newPos.X -= _dragStartFormRelative.X;
                        newPos.Y -= _dragStartFormRelative.Y;
                    }

                    this.Location = newPos;

                    // Update es muy agresivo, prueba sin él o usa Refresh si ves artefactos.
                    this.Parent?.Update();
                }
            }
        }


        private void Common_MouseUp(object sender, MouseEventArgs e) => _isPosibleDrag = false;

        private void Common_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!IsMaximized)
            {
                SuspendLayout();
                ExpandRequested?.Invoke(this, EventArgs.Empty);
                ResumeLayout();
                IsMaximized = true;
            }

            else
            {
                SuspendLayout();
                ContractRequested?.Invoke(this, EventArgs.Empty);
                IsMaximized = false;
                ResumeLayout();
            }
        }

        #endregion

        #region === Lógica del Contenedor (SetContainer) ===

        public void SetDashboardControl(Panel panel)
        {
            _parentContainer = panel ?? throw new ArgumentNullException(nameof(panel));

            // Desuscribir evento anterior si existía cambio de contenedor
            if (_parentContainer != null)
            {
                _parentContainer.SizeChanged -= ContainerSizeChanged;
            }


            EnableDoubleBuffering(_parentContainer);
            _parentContainer.SizeChanged += ContainerSizeChanged;

            // Ajuste inicial si es necesario
            this.TopLevel = false;
            this.Parent = _parentContainer;
            this.Dock = DockStyle.None; // O Fill, dependiendo de tu lógica, pero tu código usaba lógica manual
        }


        private void ContainerSizeChanged(object sender, EventArgs e)
        {
            if (IsExpanded && _parentContainer != null)
            {
                this.SuspendLayout();
                this.Size = _parentContainer.Size;
                this.Location = new Point(0, 0); // Asegurar que quede en 0,0
                this.ResumeLayout();
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            //Dar foco al form activo
            base.OnEnter(e);
            this.BringToFront();

            // Opcional: Si quiero un efecto visual (ej. cambiar color del borde al tener foco)
            // this.BorderColor = Color.Cyan; 
        }

        #endregion
 
        #region === WndProc (Redimensionado desde bordes) ===

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HTCLIENT)
                {
                    if (IsExpanded) return;

                    Point screenPoint = new Point(m.LParam.ToInt32());
                    Point clientPoint = this.PointToClient(screenPoint);

                    if (clientPoint.Y <= _resizeBorder)
                    {
                        if (clientPoint.X <= _resizeBorder) m.Result = (IntPtr)HTTOPLEFT;
                        else if (clientPoint.X >= (this.Size.Width - _resizeBorder)) m.Result = (IntPtr)HTTOPRIGHT;
                        else m.Result = (IntPtr)HTTOP;
                    }
                    else if (clientPoint.Y >= (this.Size.Height - _resizeBorder))
                    {
                        if (clientPoint.X <= _resizeBorder) m.Result = (IntPtr)HTBOTTOMLEFT;
                        else if (clientPoint.X >= (this.Size.Width - _resizeBorder)) m.Result = (IntPtr)HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)HTBOTTOM;
                    }
                    else if (clientPoint.X <= _resizeBorder) m.Result = (IntPtr)HTLEFT;
                    else if (clientPoint.X >= (this.Size.Width - _resizeBorder)) m.Result = (IntPtr)HTRIGHT;
                }
                return;
            }

            base.WndProc(ref m);
        }

        #endregion

        #region === Estilos Visuales (Virtuales) ===

        /// <summary>
        /// Método virtual que se dispara cuando el foco del formulario cambia.
        /// Sobrescribir en los hijos o implementar aquí la lógica de DarkTheme si tienes acceso a ella.
        /// </summary>
        protected virtual void OnFormActiveStateChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void deudaTecnicaVerPorQuéBotonesNoPintanHover()
        {

            btnClose.ForeColor = Darken(InternalPalette.TextSecondary, -0.4);
            Color hoverColor = Darken(InternalPalette.LowAccent, 0.2);
            Color pressColor = InternalPalette.LowAccent;

            btnClose.FlatAppearance.MouseOverBackColor = hoverColor;
            btnClose.FlatAppearance.MouseDownBackColor = pressColor;
            btnExpand.FlatAppearance.MouseOverBackColor = hoverColor;
            btnExpand.FlatAppearance.MouseDownBackColor = pressColor;
            btnMinimize.FlatAppearance.MouseOverBackColor = hoverColor;
            btnMinimize.FlatAppearance.MouseDownBackColor = pressColor;

        }
    }
}
#endregion