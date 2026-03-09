using Presenter.HostFormActions;
using Presenter.Messaging;  // Para IMessenger (asumiendo que está en Presenter; si es Shared, cambialo a Shared.Messaging)
using Shared.ArchitecturalMarkers;
using Shared.Enums;
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Presenter.MainFormNavigation
{
    public class MainFormNavigationPresenter : IDisposable
    {
        private readonly IMainFormNavigation _view;
        private LayoutType _currentLayoutType;
        private List<HostFormActionsPresenter> _hostPresenters;
        private readonly ISessionProvider _sessionProvider;
        private readonly ISessionManager _sessionManager;
        private readonly IMessenger _messenger;  // Nuevo: Inyectado para escuchar cierres
        // Bandera para detectar llamadas redundantes a Dispose
        private bool _disposed = false;

        public MainFormNavigationPresenter
              (
                  IMainFormNavigation view,
                  ISessionProvider sessionProvider,
                  ISessionManager sessionManager,
                  IMessenger messenger
              )
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _hostPresenters = new List<HostFormActionsPresenter>();
            WireViewEvents();
            _view.Showed += (sender, e) => MainMenuFirstTimeShow();

            _messenger.Subscribe<HostFormClosedMessage>(OnHostFormClosed);
        }

        // Nuevo: Handler para remover de la lista al cerrar un HostForm
        private void OnHostFormClosed(HostFormClosedMessage message)
        {
            Debugger.Break();
            var closedFormTitle = message.Payload;
            var toRemove = _hostPresenters.FirstOrDefault(p => p.FormTitle == closedFormTitle);  // Asume que agregaste FormTitle en HostFormActionsPresenter
            if (toRemove != null)
            {
                _hostPresenters.Remove(toRemove);
                ReorganizeLayout();
            }
        }

        private void MainMenuFirstTimeShow() //Probando
        {
            _view.SetStatusBarInfo
            (
               loggedUserName: _sessionProvider.Current.Id.ToString(),
               currentUserName: _sessionProvider.Current.LoginTime.ToString()
            );
        }

        private void WireViewEvents()
        {
            _view.CloseRequested += OnCloseRequested;
            _view.MinimizeRequested += OnMinimizeRequested;
            _view.ExpandContractRequested += OnExpandContractRequested;
            _view.UpdatingTitleRequested += OnUpdatingTitleRequested;
            _view.ApplyLayoutRequested += OnTileWindowsRequested;
            _view.OnResizingWindow += OnResizingWindow;
            _view.InternalWindowCreated += CreatingHostForm;
            _view.ChangeWindowManagementMode += OnChangeWindowManagementMode;
            _view.OpenClientModuleRequested += OnOpenClientModuleRequested;
            _view.OpenEmployeeModuleRequested += OnOpenEmployeeModuleRequested;
            _view.OpenSaleModuleRequested += OnOpenSaleModuleRequested;
            _view.OpenPaymentModuleRequested += OnOpenPaymentModuleRequested;
            _view.OpenProductsModuleRequested += OnOpenProductsModuleRequested;
            _view.OpenSuppliersModuleRequested += OnOpenSuppliersModuleRequested;
            _view.OpenConfigsModuleRequested += OnOpenConfigsModuleRequested;
        }

        private void UnwireViewEvents()
        {
            if (_view == null) return;

            _view.CloseRequested -= OnCloseRequested;
            _view.MinimizeRequested -= OnMinimizeRequested;
            _view.ExpandContractRequested -= OnExpandContractRequested;
            _view.UpdatingTitleRequested -= OnUpdatingTitleRequested;
            _view.ApplyLayoutRequested -= OnTileWindowsRequested;
            _view.OnResizingWindow -= OnResizingWindow;
            _view.InternalWindowCreated -= CreatingHostForm;
            _view.ChangeWindowManagementMode -= OnChangeWindowManagementMode;
            _view.OpenClientModuleRequested -= OnOpenClientModuleRequested;
            _view.OpenEmployeeModuleRequested -= OnOpenEmployeeModuleRequested;
            _view.OpenSaleModuleRequested -= OnOpenSaleModuleRequested;
            _view.OpenPaymentModuleRequested -= OnOpenPaymentModuleRequested;
            _view.OpenProductsModuleRequested -= OnOpenProductsModuleRequested;
            _view.OpenSuppliersModuleRequested -= OnOpenSuppliersModuleRequested;
            _view.OpenConfigsModuleRequested -= OnOpenConfigsModuleRequested;
        }

        // ==========================================
        // MANEJADORES DE EVENTOS DE LA VISTA
        // ==========================================
        private void OnCloseRequested(object sender, EventArgs e) => _view.CloseApp();
        private void OnMinimizeRequested(object sender, EventArgs e) => _view.MinimizeWindow();
        private void OnExpandContractRequested(object sender, EventArgs e)
        {
            _view.UpdateWindowState();
            ReorganizeLayout();
        }
        private void OnUpdatingTitleRequested(object sender, EventArgs e) => _view.UpdateTitle();
        private void OnResizingWindow(object sender, EventArgs e) => OnResizingMainForm();
        private void OnChangeWindowManagementMode(object sender, EventArgs e)
        {
            _view.WindowManageMode = (_view.WindowManageMode == WindowManagementMode.Tabbed)
                ? WindowManagementMode.Dashboard
                : WindowManagementMode.Tabbed;
            ReorganizeLayout();
            _view.UpdateWindowManageText();
        }
        private void OnOpenClientModuleRequested(object sender, EventArgs e) => _view.OpenClientsFrm();
        private void OnOpenEmployeeModuleRequested(object sender, EventArgs e) => _view.OpenEmployeeFrm();
        private void OnOpenSaleModuleRequested(object sender, EventArgs e) => _view.OpenSaleFrm();
        private void OnOpenPaymentModuleRequested(object sender, EventArgs e) => _view.OpenPaymentFrm();
        private void OnOpenProductsModuleRequested(object sender, EventArgs e) => _view.OpenProductFrm();
        private void OnOpenSuppliersModuleRequested(object sender, EventArgs e) => _view.OpenSupplierFrm();
        private void OnOpenConfigsModuleRequested(object sender, EventArgs e) => _view.OpenConfigsFrm();

        // ==========================================
        // LÓGICA DE NEGOCIO Y PRESENTACIÓN
        // ==========================================
        private void CreatingHostForm(object sender, IHostFormActions internalWindow)
        {
            HostFormActionsPresenter hfaPresenter = new HostFormActionsPresenter(internalWindow); // Una vez creada la lógica de ventanas internas, crear Factory para pasarle el IHostFormActions y obtener el Presenter correspondiente.
            _hostPresenters.Insert(0, hfaPresenter);
            ReorganizeLayout();
            // Nota: Las lambdas aquí están bien si HostFormActionsPresenter destruye sus
            // propios eventos internamente, o si mueren junto con la app.
            hfaPresenter.OnMinimizingWindow += (s, e) => ReorganizeLayout();
            hfaPresenter.OnRestoringFromMinimized += (s, e) =>
            {
                if (_hostPresenters.Contains(hfaPresenter))
                {
                    _hostPresenters.Remove(hfaPresenter);
                    _hostPresenters.Insert(0, hfaPresenter);
                }
                else
                {
                    _hostPresenters.Insert(0, hfaPresenter);
                }
                ReorganizeLayout();
            };
            hfaPresenter.OnExpandingWindow += (s, e) =>
            {
                if (_view.WindowManageMode == WindowManagementMode.Tabbed)
                    return;
                MinimizeAllActiveWindows();
            };
        }

        private void MinimizeAllActiveWindows()
        {
            if (_hostPresenters.Count == 0) return;
            var excepted = _hostPresenters[0];
            foreach (var window in _hostPresenters)
            {
                if (window != excepted && window.IsMinimized == false)
                {
                    window.MinimizeWindow();
                    window.SetMaximizeStatus(false);
                }
            }
        }

        private void ReorganizeLayout()
        {
            var activeWindows = _view.GetActiveInternalWindows();
            if (_view.WindowManageMode == WindowManagementMode.Tabbed)
            {
                if (activeWindows.ToList().Count < 1 || _hostPresenters.Count == 0) return;
                var excepted = _hostPresenters[0];
                MinimizeAllActiveWindows();
                excepted.ExpandWindow();
                excepted.SetMaximizeStatus(true);
            }
            if (_view.WindowManageMode == WindowManagementMode.Dashboard)
            {
                foreach (var window in _hostPresenters)
                {
                    window.SetMaximizeStatus(false);
                }
                _view.TileWindows(_currentLayoutType, activeWindows);
            }
        }

        private void OnResizingMainForm()
        {
            var activeWindows = _view.GetActiveInternalWindows();
            _view.TileWindows(_currentLayoutType, activeWindows);
        }

        private void OnTileWindowsRequested(object sender, LayoutType layoutType)
        {
            _view.CurrentLayoutType = layoutType;
            _currentLayoutType = _view.CurrentLayoutType;
            var activeWindows = _view.GetActiveInternalWindows();
            if (activeWindows != null && activeWindows.Any())
            {
                _view.TileWindows(_currentLayoutType, activeWindows);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _messenger.Unsubscribe<HostFormClosedMessage>(OnHostFormClosed);  // Ahora funciona con el constraint correcto
                    UnwireViewEvents();
                    if (_hostPresenters != null)
                    {
                        foreach (var presenter in _hostPresenters)
                        {
                            if (presenter is IDisposable disposablePresenter)
                            {
                                disposablePresenter.Dispose();
                            }
                        }
                        _hostPresenters.Clear();
                    }
                }
                _disposed = true;
            }
        }
    }
}