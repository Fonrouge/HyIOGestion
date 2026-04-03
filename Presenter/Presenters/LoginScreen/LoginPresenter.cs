using BLL.UseCases;
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
        }

        private async Task OnLoginRequestedHandler(object sender, (string username, string password) credentials)
        {
            try
            {
                _view.SetLoadingState(true);
                var operationResult = await _loginUseCase.ExecuteAsync(credentials.username, credentials.password);                
                _view.ShowOperationResult(operationResult);
            }
            finally
            {
                _view.SetLoadingState(false);
            }
        }

        public void Dispose()
        {
            _view.LoginRequested -= OnLoginRequestedHandler;
        }
    }
}