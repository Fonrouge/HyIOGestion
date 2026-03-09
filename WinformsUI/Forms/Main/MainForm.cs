using BLL.SessionInfo;
using Presenter.ForClient;
using Presenter.ForEmployee;
using Presenter.ForPayments;
using Presenter.ForProducts;
using Presenter.ForSale;
using Presenter.ForSupplier;
using Presenter.MainFormNavigation;
using Shared.ArchitecturalMarkers;
using Shared.Enums;
using Shared.Factories;
using Shared.Sessions;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Winforms.Theme;
using WinformsUI.Forms.ConfigurationsMenu;
using WinformsUI.Forms.Base;
using WinformsUI.Infrastructure;
using WinformsUI.Infrastructure.Factories;
using WinformsUI.Infrastructure.Translations;
using WinformsUI.Infrastructure.UserInterface.Windowing;
using WinformsUI.Properties;
using WinformsUI.UserControls.CustomDGV;
using static Winforms.Theme.DarkTheme;

namespace WinformsUI.Forms.Main
{
    /// <summary>
    /// Represents the main application window. 
    /// Acts as a Passive View in the Model-View-Presenter (MVP) pattern, 
    /// delegating business logic and navigation decisions to the Presenter.
    /// </summary>
    public partial class MainForm : Form, IMainFormNavigation
    {
        private readonly IFormsFactory _formsFactory;
        private readonly IAppEnvironmentFactory _appEnvFactory;
        private readonly ILayoutStrategyFactory _layoutFactory;
        private readonly ICustomDGVFactory _customDgvFact;
        private readonly ITranslatableControlsManager _transMgr;


        public LayoutType CurrentLayoutType { get; set; }

        public WindowManagementMode WindowManageMode { get; set; }

        private readonly string _defaultTitle = "<EXAMPLE TITLE>";


        //ON PRODUCTION, THESE STRINGS SHOULD BE RETRIEVED FROM A RESOURCE FILE FOR LOCALIZATION PURPOSES. THEY ARE HARDCODED HERE FOR SIMPLICITY AND DEMONSTRATION.
#pragma warning disable IDE0044 // These fields are translated at runtime
        private string _employeeTitle = "Empleados";
        private string _paymentsTitle = "Pagos";
        private string _salesTitle = "Ventas";
        private string _clientsTitle = "Clientes";
        private string _productsTitle = "Productos";
        private string _suppliersTitle = "Proveedores";

        private string _dashboardModeText = "Modo tablero";
        private string _tabbedModeText = "Modo pestañas";
#pragma warning restore IDE0044

        // Mapea la instancia del form con su LLAVE de traducción (ej: "MainForm._employeeTitle")
        private Dictionary<IHostFormActions, string> _formTranslationKeys = new Dictionary<IHostFormActions, string>();


        #region Events (View to Presenter Communication)

        /// <summary>Fired when the user requests to close the application.</summary>
        public event EventHandler CloseRequested;

        /// <summary>Fired when the user requests to minimize the window.</summary>
        public event EventHandler MinimizeRequested;

        /// <summary>Fired when the user requests to expand or contract the window state.</summary>
        public event EventHandler ExpandContractRequested;

        /// <summary>Fired when a window tiling layout is requested.</summary>
        public event EventHandler<LayoutType> ApplyLayoutRequested;

        //Fired when a module is requested        
        public event EventHandler OpenClientModuleRequested;
        public event EventHandler OpenSaleModuleRequested;
        public event EventHandler OpenEmployeeModuleRequested;
        public event EventHandler OpenPaymentModuleRequested;
        public event EventHandler OpenProductsModuleRequested;
        public event EventHandler OpenSuppliersModuleRequested;
        public event EventHandler OpenConfigsModuleRequested;

        /// <summary>Notifcation on creation of a form to wire it to it's forms events on Presenter layer </summary>
        public event EventHandler<IHostFormActions> InternalWindowCreated;

        public event EventHandler ChangeWindowManagementMode;
        public event EventHandler OnResizingWindow;
        public event EventHandler UpdatingTitleRequested;
        public event EventHandler Showed;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        /// <param name="formsFactory">Factory for creating internal forms.</param>
        /// <param name="appEnvFactory">Factory for initializing dashboard and tools environment.</param>
        /// <param name="layoutFactory">Factory for window arrangement strategies.</param>
        public MainForm
        (
            IFormsFactory formsFactory,
            IAppEnvironmentFactory appEnvFactory,
            ILayoutStrategyFactory layoutFactory,
            ICustomDGVFactory customDgvFact,
            ITranslatableControlsManager transMgr
        )
        {
            _formsFactory = formsFactory ?? throw new ArgumentNullException(nameof(formsFactory));
            _appEnvFactory = appEnvFactory ?? throw new ArgumentNullException(nameof(appEnvFactory));
            _layoutFactory = layoutFactory ?? throw new ArgumentNullException(nameof(layoutFactory));
            _customDgvFact = customDgvFact ?? throw new ArgumentNullException(nameof(customDgvFact));
            _transMgr = transMgr ?? throw new ArgumentNullException(nameof(transMgr));


            InitializeComponent();

            InitializeEnvironment();
            ApplyGlobalPalette();
            ConfigureInitialState();
            WireGeneralEvents();
            AddTranslatables();

            this.Load += (sender, e) => Showed?.Invoke(this, EventArgs.Empty);
            this.FormClosed += (sender, e) => _transMgr.RemoveFormNotify(this);

        }


        public void SetStatusBarInfo(string loggedUserName, string currentUserName) //Probando
        {
            txtLoginTime.Text = currentUserName;
            txtCurrentUserName.Text = loggedUserName;
        }

        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");


            _transMgr.AddString("MainForm.Textbox.tbmt", _tabbedModeText);

            _transMgr.AddString("MainForm._employeeTitle", _employeeTitle);
            _transMgr.AddString("MainForm._paymentsTitle", _paymentsTitle);
            _transMgr.AddString("MainForm._salesTitle", _salesTitle);
            _transMgr.AddString("MainForm._clientsTitle", _clientsTitle);
            _transMgr.AddString("MainForm._productsTitle", _productsTitle);
            _transMgr.AddString("MainForm._suppliersTitle", _suppliersTitle);

            _transMgr.AddString("MainForm.Textbox.dbmt", _dashboardModeText);
            _transMgr.AddString("MainForm.Textbox.tbmt", _tabbedModeText);

            _transMgr.AddSingleObject(txtSuppliers, "Text");

            //Para después hacer el OnClose(); ---> _transMgr.RemoveFormNotify(this);


            ApplyTranslation();



            _transMgr.AddFormNotify(this);
        }

        //It's triggered by reflection, that's why it looks like no-referenced.
        public void NotifiedByTranslationManager()
        {
            foreach (var entry in _formTranslationKeys)
            {
                IHostFormActions hostForm = entry.Key;
                string translationKey = entry.Value;

                hostForm.SetTitle(_transMgr.GetString(translationKey));
            }

            ApplyTranslation();
        }

        public void ApplyTranslation() => _transMgr.Apply();

        #region Initialization

        /// <summary>Configures initial window properties such as constraints and default text.</summary>
        private void ConfigureInitialState()
        {
            this.MaximumSize = Screen.FromControl(this).WorkingArea.Size;
            tlpMenu.Size = tlpMenu.MinimumSize;
            UpdatingTitleRequested?.Invoke(this, EventArgs.Empty);
            CurrentLayoutType = LayoutType.VerticalTile;

            WindowManageMode = WindowManagementMode.Dashboard;
            UpdateWindowManageText();
        }

        /// <summary>Initializes the UI environment (Dashboard and Side Panels) through the environment factory.</summary>
        private void InitializeEnvironment() =>
            _appEnvFactory.SetMainContainers(DashboardPnl, FLPsideTools);


        /// <summary>Applies the global dark theme palette to the form. Static class on purpose, it's a Drag&Drop library.</summary>
        private void ApplyGlobalPalette()
        {
            Color darkerAccent = Darken(DarkTheme.GetCurrentPalette().LowAccent, 0.85);

            DarkTheme.RedrawBorders = false;
            PaintControlTree(tlpMenu, GetCurrentPalette());
            PaintControlTree(FLPsideTools, GetCurrentPalette());

            DarkTheme.ApplyGradientBackground(tlpMenu, darkerAccent, darkerAccent, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            DarkTheme.ApplyGradientBackground(tableLayoutPanel1, darkerAccent, darkerAccent, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            mainSs.BackColor = darkerAccent;
        }

        private void ExecuteSingleInvoke(Action m)
        {
            SuspendLayout();
            m();
            ResumeLayout();
        }

        /// <summary>Wires UI control events to trigger View-to-Presenter notifications.</summary>
        private void WireGeneralEvents()
        {
            // Title bar actions
            btnExpand.Click += (s, e) => ExecuteSingleInvoke(() => ExpandContractRequested?.Invoke(this, EventArgs.Empty));
            btnMinimize.Click += (s, e) => ExecuteSingleInvoke(() => MinimizeRequested?.Invoke(this, EventArgs.Empty));
            btnClose.Click += (s, e) => ExecuteSingleInvoke(() => CloseRequested?.Invoke(this, EventArgs.Empty));

            // Menu actions
            btnMenu.Click += (s, e) => CollapseExpandMenu();

            // Window management actions
            btnVerticalTileWindows.Click += (s, e) => ExecuteSingleInvoke(() => ApplyLayoutRequested?.Invoke(this, LayoutType.VerticalTile));

            btnChangeWindowManagementMode.Click += (s, e) => ExecuteSingleInvoke(() => ChangeWindowManagementMode?.Invoke(this, EventArgs.Empty));

            // Module navigation actions
            btnOpenClients.Click += (s, e) =>
            {
                SuspendLayout();
                OpenClientModuleRequested?.Invoke(this, EventArgs.Empty);
                ApplyLayoutRequested?.Invoke(this, CurrentLayoutType);
                ResumeLayout();
            };
            btnOpenEmployees.Click += (s, e) =>
            {
                SuspendLayout();
                OpenEmployeeModuleRequested?.Invoke(this, EventArgs.Empty);
                ApplyLayoutRequested?.Invoke(this, CurrentLayoutType);
                ResumeLayout();
            };
            btnOpenPayments.Click += (s, e) =>
            {
                SuspendLayout();
                OpenPaymentModuleRequested?.Invoke(this, EventArgs.Empty);
                ApplyLayoutRequested?.Invoke(this, CurrentLayoutType);
                ResumeLayout();
            };
            btnOpenSales.Click += (s, e) =>
            {
                SuspendLayout();
                OpenSaleModuleRequested?.Invoke(this, EventArgs.Empty);
                ApplyLayoutRequested?.Invoke(this, CurrentLayoutType);
                ResumeLayout();
            };
            btnOpenProducts.Click += (s, e) =>
            {
                SuspendLayout();
                OpenProductsModuleRequested?.Invoke(this, EventArgs.Empty);
                ApplyLayoutRequested?.Invoke(this, CurrentLayoutType);
                ResumeLayout();
            };
            btnOpenSuppliers.Click += (s, e) =>
            {
                SuspendLayout();
                OpenSuppliersModuleRequested?.Invoke(this, EventArgs.Empty);
                ApplyLayoutRequested?.Invoke(this, CurrentLayoutType);
                ResumeLayout();
            };
            btnOpenConfigs.Click += (s, e) =>
            {
                SuspendLayout();
                OpenConfigsModuleRequested?.Invoke(this, EventArgs.Empty);
                ApplyLayoutRequested?.Invoke(this, CurrentLayoutType);
                ResumeLayout();
            };

            this.Resize += (s, e) => ExecuteSingleInvoke(() => OnResizingWindow?.Invoke(this, EventArgs.Empty));

        }

        #endregion

        public void UpdateTitle()
        {
            this.Text = _defaultTitle;
        }


        public void UpdateWindowManageText()
        {
            btnChangeWindowManagementMode.Text = (WindowManageMode == WindowManagementMode.Tabbed)
                ? _tabbedModeText
                : _dashboardModeText;
        }


        #region UI Logic

        /// <summary>Handles the visual expansion or collapse of the side menu.</summary>
        private void CollapseExpandMenu()
        {
            tlpMenu.Size = (tlpMenu.Size.Width == tlpMenu.MinimumSize.Width)
                ? tlpMenu.MaximumSize
                : tlpMenu.MinimumSize;
        }

        #endregion



        /// <inheritdoc/>
        public void SetTitle(string title) => this.Text = title;

        /// <inheritdoc/>
        public void CloseApp() => Application.Exit(); //Lack of verifications for open/working modules (for future iterations)

        /// <inheritdoc/>
        public void MinimizeWindow() =>
            this.WindowState = FormWindowState.Minimized;

        /// <inheritdoc/>
        public void UpdateWindowState() =>
            this.WindowState = this.WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;

        /// <inheritdoc/>
        public void TileWindows(LayoutType layoutType, IEnumerable<IHostFormActions> objsForTiling)
        {
            SuspendLayout();
            _layoutFactory.Create(layoutType).Arrange(DashboardPnl.Bounds, (IEnumerable<Form>)objsForTiling);
            ResumeLayout();
        }


        /// <summary>
        /// Retrieves active hosted forms that implement <see cref="IHostFormActions"/>.
        /// Used by the Presenter to manage window state without accessing private controls.
        /// </summary>
        /// <returns>A collection of active, non-disposed internal windows.</returns>
        public IEnumerable<IHostFormActions> GetActiveInternalWindows()
        {
            return DashboardPnl.Controls.OfType<HostForm>()
                .Where(hf => hf.Visible && !hf.IsDisposed && !hf.IsMinimized)
                .Cast<IHostFormActions>();
        }

        public void OpenClientsFrm()
        {
            SuspendLayout();
            CreateForm<IClientView>
            (
                DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Obsidian() : DarkTheme.PalettesLight.Classic(),
                FormTypeEnum.Module,
                Resources.BusinessPartnerIcon,
                "MainForm._clientsTitle",
                () => _formsFactory.ClientForm<IClientView>()
            );
            ResumeLayout();
        }

        public void OpenEmployeeFrm()
        {
            CreateForm<IEmployeeView>
            (
                DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Nordic() : DarkTheme.PalettesLight.Paper(),
                FormTypeEnum.Module,
                Resources.EmployeeIcon,
                "MainForm._employeeTitle",
                () => _formsFactory.EmployeeForm<IEmployeeView>()
            );
        }

        public void OpenSaleFrm()
        {
            CreateForm<ISaleView>
            (
                DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Forest() : DarkTheme.PalettesLight.Mint(),
                FormTypeEnum.Module,
                Resources.SaleIcon,
                "MainForm._salesTitle",
                () => _formsFactory.SaleForm<ISaleView>()
            );
        }

        public void OpenPaymentFrm()
        {
            CreateForm<IPaymentView>
            (
                DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Volcanic() : DarkTheme.PalettesLight.Berry(),
                FormTypeEnum.Module,
                Resources.PaymentsIcon,
                "MainForm._paymentsTitle",
                () => _formsFactory.PaymentForm<IPaymentView>()
            );
        }

        public void OpenProductFrm()
        {

            CreateForm<IProductView>
            (
                DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Aubergine() : DarkTheme.PalettesLight.Grape(),
                FormTypeEnum.Module,
                Resources.ProductsIcon,
                "MainForm._productsTitle",
                () => _formsFactory.ProductForm<IProductView>()
            );
        }

        public void OpenSupplierFrm()
        {
            CreateForm<ISupplierView>
            (
                DarkTheme.IsDarkPalette(DarkTheme.GetCurrentPalette()) ? DarkTheme.PalettesDark.Oceanic() : DarkTheme.PalettesLight.Solar(),
                FormTypeEnum.Module,
                Resources.SuppliersIcon,
                "MainForm._suppliersTitle",
                () => _formsFactory.SupplierForm<ISupplierView>()
            );
        }

        public void OpenConfigsFrm()
        {
            _formsFactory.CreateGeneric<ConfigurationsForm>().ShowDialog();
        }

        public void CreateForm<TView>
        (
            DarkTheme.Palette p,
            FormTypeEnum fte,
            Bitmap icon,
            string translationKey,
            Func<TView> viewProvider) where TView : class, IView
        {
            TView view = viewProvider();
            if (!(view is Form content))
                throw new InvalidOperationException($"La vista {typeof(TView).Name} no hereda de Form");
            
            string currentTitle = _transMgr.GetString(translationKey) ?? translationKey;
            
            System.Diagnostics.Trace.WriteLine($"Creando form con título: {currentTitle}");  // Log para debug
            
            IAppEnvironment environment = _appEnvFactory.CreateCustom
            (
                DashBoard: DashboardPnl,
                SlotForTabs: FLPsideTools,
                Palette: p,
                FormType: (int)fte,
                Icon: icon
            );
            IHostFormActions hostFrm = _formsFactory.CreateHFA
            (
                Title: currentTitle,
                Environment: environment
            );
            hostFrm.SetTitle(currentTitle);  
                                             
            _formTranslationKeys.Add(hostFrm, translationKey);
            InternalWindowCreated?.Invoke(this, hostFrm);
            hostFrm.SetContent(content);
            this.ShowForm(hostFrm);
        }

        /// <summary>Injects a form into the dashboard container and displays it.</summary>
        /// <param name="F">The form to be hosted.</param>
        private void ShowForm(IHostFormActions F)
        {
            if (F is Form form)
            {
                form.TopLevel = false;
                DashboardPnl.Controls.Add(form);
                form.BringToFront();
                form.Show();
            }
        }


    }
}