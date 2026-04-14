using BLL.DTOs;
using BLL.LogicLayers;
using BLL.LogicLayers.User.UseCases;
using Presenter.Messaging;
using Shared.Sessions;
using System;
using System.Threading.Tasks;

namespace Presenter.Presenters.ForUser
{
    public class UserPresenter
    {
        private readonly IUCUpdateUser _uCUpdateUser;
        private readonly IUCGetUserById _uCGetUser; 
        private readonly IMessenger _messenger;
        private readonly ISessionProvider _session;

        private UsuarioDTO _currentUser;

        public UserPresenter
        (
            IUCUpdateUser uCUpdateUser,
            IUCGetUserById uCGetUser, 
            IMessenger messenger,
            ISessionProvider session
        )
        {
            _uCUpdateUser = uCUpdateUser ?? throw new ArgumentNullException(nameof(uCUpdateUser));
            _uCGetUser = uCGetUser ?? throw new ArgumentNullException(nameof(uCGetUser)); 
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _session = session ?? throw new ArgumentNullException(nameof(session));

            SubscribeToMessenger();
        }

        private void SubscribeToMessenger()
        {
            _messenger.Subscribe<TranslationNotificationMessage>(async msg => await UpdateUserPrefferLanguage(msg));
        }

        private async Task UpdateUserPrefferLanguage(TranslationNotificationMessage message)
        {
            await GetUser();

            if (_currentUser != null && !string.IsNullOrEmpty(message?.Payload))
            {
                _currentUser.LanguageCode = message.Payload;
                await _uCUpdateUser.ExecuteAsync(_currentUser);
            }
        }

        private async Task GetUser()
        {
            var currentSession = _session.Current;
            if (currentSession == null) return;

            var tuple = await _uCGetUser.ExecuteAsync(currentSession.CurrentUserId);

            if (tuple.Item1 != null)
            {
                _currentUser = tuple.Item1;
            }
        }
    }
}
