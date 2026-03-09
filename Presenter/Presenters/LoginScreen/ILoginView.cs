using BLL.DTOs;
using System;
using System.Threading.Tasks;

namespace Presenter.LoginScreen
{
    public interface ILoginView : IDisposable
    {
        event Func<object, (string username, string password), Task> LoginRequested;

        event EventHandler CloseRequested;
        event EventHandler MinimizeRequested;
        event EventHandler ContractRequested;

        bool ShowDialog(); // ShowDialog suele devolver bool? en WPF/WinForms
        void ShowOperationResult(OperationResult<UsuarioDTO> result);
        void SetLoadingState(bool isLoading);
        void Close();
    }
}