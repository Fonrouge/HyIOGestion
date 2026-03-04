using BLL.DTOs;
using BLL.UseCases;
using Shared.Sessions;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Threading.Tasks;

namespace Presenter.LoginScreen
{
    public class LoginPresenter : IPresenter, IDisposable
    {
        private readonly ILoginView _view;
        private readonly IUCLogin _loginUseCase;


        public LoginPresenter
        (
            ILoginView view,
            IUCLogin loginUseCase
        )
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _loginUseCase = loginUseCase ?? throw new ArgumentNullException(nameof(loginUseCase));                                    

            WireEvents();
        }

        private void WireEvents()
        {
            _view.LoginRequested += OnLoginRequestedHandler;

            //For more events...
        }

        private async Task OnLoginRequestedHandler(object sender, (string username, string password) credentials)
        {
            try
            {
                _view.SetLoadingState(true);

                var operationResult = await _loginUseCase.ExecuteAsync(credentials.username, credentials.password);


                
                _view.ShowOperationResult(operationResult);
            }
            catch (Exception ex)
            {
                var opResultException = OperationResult<UsuarioDTO>.FromException(ex);
                _view.ShowOperationResult(opResultException);
            }
            finally
            {
                _view.SetLoadingState(false);
            }
        }

        public void Dispose()
        {
            // Desuscripción para evitar memory leaks. Desuscribir de todos los eventos a los que se haya suscripto el presenter.
            _view.LoginRequested -= OnLoginRequestedHandler;
        }
    }
}