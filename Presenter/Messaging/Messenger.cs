using Presenter.Messaging;
using System;
using System.Collections.Generic;

namespace Presenter.Messaging  
{
    public class Messenger : IMessenger
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _handlers = new Dictionary<Type, List<Action<IMessage>>>();  // Cambiado: Strong refs, sin WeakReference

        public void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessage
        {
            var messageType = typeof(TMessage);
            if (!_handlers.TryGetValue(messageType, out var list))
            {
                list = new List<Action<IMessage>>();
                _handlers[messageType] = list;
            }

            // Wrap en Action<IMessage> para uniformidad
            Action<IMessage> wrapped = msg => handler((TMessage)msg);
            list.Add(wrapped);  // Agrega directamente (strong)
        }

        public void Unsubscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessage
        {
            var messageType = typeof(TMessage);
            if (_handlers.TryGetValue(messageType, out var list))
            {
                // Buscar y remover el wrapped
                Action<IMessage> wrapped = msg => handler((TMessage)msg);
                list.RemoveAll(target => target == wrapped);  // Remueve matches directos
            }
        }

        public void Send<TMessage>(TMessage message) where TMessage : IMessage
        {
            var messageType = typeof(TMessage);
            if (_handlers.TryGetValue(messageType, out var list))
            {
                // Copia para evitar modifs durante iteración
                var copy = list.ToArray();

                foreach (var handler in copy)
                {
                    handler(message);  // Llama directamente (sin TryGetTarget/else)
                }
            }
        }
    }
}