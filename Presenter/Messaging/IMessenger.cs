using System;
using System.Runtime.Remoting.Messaging;

namespace Presenter.Messaging
{
    public interface IMessenger
    {
        // Suscribirse a un tipo de mensaje
        void Subscribe<T>(Action<T> handler) where T : IMessage;

        // Desuscribirse
        void Unsubscribe<T>(Action<T> handler) where T : IMessage;

        // Enviar un mensaje
        void Send<T>(T message) where T : IMessage;
    }
}