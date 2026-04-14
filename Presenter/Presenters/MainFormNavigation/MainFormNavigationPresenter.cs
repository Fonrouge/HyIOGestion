using BLL.DTOs;
using BLL.LogicLayers.User.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Presenter.HostFormActions;
using Presenter.Messaging;
using Shared.ArchitecturalMarkers;
using Shared.Enums;
using Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presenter.MainFormNavigation
{
    public class MainFormNavigationPresenter : IDisposable
    {
        private readonly IMainFormNavigation _view;
        private LayoutTypeEnum _currentLayoutType;
        private List<HostFormActionsPresenter> _hostPresenters;
        private readonly ISessionProvider _sessionProvider;
        private readonly IMessenger _messenger;
        private readonly IUCGetUserById _uCGetUserById;
        private readonly IServiceProvider _sp;
        private UsuarioDTO _userDto;

        private bool _disposed = false;

        public MainFormNavigationPresenter
        (
            IMainFormNavigation view,
            ISessionProvider sessionProvider,
            IMessenger messenger,
            IUCGetUserById uCGetUserById,
            IServiceProvider sp
        )
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _uCGetUserById = uCGetUserById ?? throw new ArgumentNullException(nameof(uCGetUserById));
            _hostPresenters = new List<HostFormActionsPresenter>();
            _sp = sp;
            WireViewEvents();
            _view.OnceLoadedAdvice += (sender, e) => Task.FromResult(MainMenuFirstTimeShow());


            _messenger.Subscribe<HostFormClosedNotificationMessage>(OnHostFormClosed);
        }

        private void OnHostFormClosed(HostFormClosedNotificationMessage message)
        {
            var closedFormId = message.Payload;            
            var toRemove = _hostPresenters.FirstOrDefault(p => p.View.GetViewId() == closedFormId);

            if (toRemove != null)
            {
                _hostPresenters.Remove(toRemove);
            }

            ReorganizeLayout();
        }

        private async Task MainMenuFirstTimeShow()
        {
            UsuarioDTO userDto = null;
            OperationResult<UsuarioDTO> opRes = null;

            try
            {
                var tuple = await _uCGetUserById.ExecuteAsync(_sessionProvider.Current.CurrentUserId);
                userDto = tuple.Item1;
                opRes = tuple.Item2;
            }
            catch (Exception ex)
            {
                _view.ShowOperationResult(opRes);
                return;
            }

            if (userDto != null)
            {
                _view.SetStatusBarInfo
                (
                   loggedUserName: userDto.Username,
                   currentUserName: _sessionProvider.Current.LoginTime.ToString()
                );
                _userDto = userDto;
            }
            else
            {
                _view.SetStatusBarInfo
                (
                   loggedUserName: "",
                   currentUserName: _sessionProvider.Current.LoginTime.ToString()
                );
            }

            var requestTranslation = new TranslationRequestMessage(userDto.LanguageCode, this);
            _messenger.Send(requestTranslation);
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
            _view.OnceLoadedAdvice += TranslateUIByUserLangCode;
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
            _view.WindowManageMode = (_view.WindowManageMode == WindowManagementModeEnum.Tabbed)
                ? WindowManagementModeEnum.Dashboard
                : WindowManagementModeEnum.Tabbed;

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
        private void TranslateUIByUserLangCode(object sender, EventArgs e)
        {
            if (_userDto == null) return;

            var requestTranslation = new TranslationRequestMessage(_userDto.LanguageCode, this);
            _messenger.Send(requestTranslation);
        }


        // ==========================================
        // LÓGICA DE NEGOCIO Y PRESENTACIÓN
        // ==========================================
        private void CreatingHostForm(object sender, IHostFormActions internalWindow)
        {
            HostFormActionsPresenter hfaPresenter = ActivatorUtilities.CreateInstance<HostFormActionsPresenter>(_sp, internalWindow);
            _hostPresenters.Insert(0, hfaPresenter);

            ReorganizeLayout();
            // Nota: Las lambdas aquí están bien si HostFormActionsPresenter destruye sus
            // propios eventos internamente, o si mueren junto con la app.
            hfaPresenter.OnMinimizingWindow -= (s, e) => ReorganizeLayout();

            hfaPresenter.OnRestoringFromMinimized += (s, e) =>
            {
                if (_hostPresenters.Contains(hfaPresenter)) //Tener clara la disquisición entre Tabbed y Dashboard para que efectivamente se pueda switchear ed ventana en modo Dashboard, pero no se pierda el correcto index en Tabbed (y se minimicen las ventanas que se tienen que minimizar cuando se expande la ventana que debe tener index 0)
                {
                    if (_view.WindowManageMode == WindowManagementModeEnum.Tabbed)
                    {
                        _hostPresenters.Remove(hfaPresenter);
                        _hostPresenters.Insert(0, hfaPresenter);
                    }
                }
                else
                {
                    _hostPresenters.Insert(0, hfaPresenter);
                }

                ReorganizeLayout();
            };

            hfaPresenter.OnExpandingWindow += (s, e) =>
            {
                if (_view.WindowManageMode == WindowManagementModeEnum.Tabbed)
                    return;

                MinimizeAllActiveWindows(s);
            };
        }

        private void MinimizeAllActiveWindows(object sender)
        {
            if (_hostPresenters.Count == 0) return;

            if (sender == null)
            {
                foreach (var window in _hostPresenters)
                {
                    if (window != _hostPresenters[0])
                    {
                        window.MinimizeWindow();
                        window.SetMaximizeStatus(false);
                    }
                }
            }
            else
            {
                var excepted = sender;

                foreach (var window in _hostPresenters)
                {
                    if (window != excepted)
                    {
                        window.MinimizeWindow();
                        window.SetMaximizeStatus(false);
                    }
                }
            }
        }

        /* MÉTODO IMPORTANTÍSIMO
           Maneja la lógica de organización de ventanas, tanto en modo Tabbed como Dashboard, teniendo en cuenta el orden de los presenters en la lista
        y el estado de cada ventana (minimizada o maximizada) para garantizar una experiencia de usuario coherente y fluida al interactuar con múltiples módulos internos.
        Si está en Modo pestaña (de a una ventana maximizada) todas son minimizadas y se actualiza su estado a IsMinimized = true.
        Si está en Modo tablero (de a varias ventanas maximizadas) todas serán actualizadas a IsMinimized = false porque de facto ninguna puede estar maximizada.
        Dado el caso de que tenga UNA sola ventana y sólo UNA está estará obligatoriamente maximizada sea cuál sea el modo elegido.*/
        private void ReorganizeLayout()
        {
            var activeWindows = _view.GetActiveInternalWindows();

            if (_view.WindowManageMode == WindowManagementModeEnum.Tabbed)
            {
                if (activeWindows.ToList().Count < 1 || _hostPresenters.Count == 0) return;

                var excepted = _hostPresenters[0];

                MinimizeAllActiveWindows(null);
                excepted.ExpandWindow();
                excepted.SetMaximizeStatus(true);
            }

            if (_view.WindowManageMode == WindowManagementModeEnum.Dashboard)
            {
                if (_hostPresenters.Count() == 1)
                {
                    foreach (var hfa in _hostPresenters)
                    {
                        hfa.SetMaximizeStatus(true);
                    }
                }

                else if (_hostPresenters.Count() > 1)
                {
                    foreach (var window in _hostPresenters)
                    {
                        window.SetMaximizeStatus(false);

                    }
                }

                _view.TileWindows(_currentLayoutType, activeWindows);
            }
        }

        private void OnResizingMainForm()
        {
            var activeWindows = _view.GetActiveInternalWindows();
            OnTileWindowsRequested(null);
        }

        private void OnTileWindowsRequested(object sender, LayoutTypeEnum layoutType = LayoutTypeEnum.VerticalTile)
        {
            _view.CurrentLayoutType = layoutType;
            _currentLayoutType = _view.CurrentLayoutType;
            var activeWindows = _view.GetActiveInternalWindows();


            if (activeWindows != null && activeWindows.Any())
            {
                _view.TileWindows(_currentLayoutType, activeWindows);
            }
        }


        // ==========================================
        // CUIDADO DE RAM (MEMORY CARE)
        // ==========================================
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _messenger.Unsubscribe<HostFormClosedNotificationMessage>(OnHostFormClosed);  // Ahora funciona con el constraint correcto

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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

    }
}