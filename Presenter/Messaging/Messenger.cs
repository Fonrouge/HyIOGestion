using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;


namespace Presenter.Messaging
{
    public class Messenger : IMessenger
    {
        private readonly Dictionary<Type, List<WeakReference<Action<IMessage>>>> _handlers = new Dictionary<Type, List<WeakReference<Action<IMessage>>>>();

        public void Subscribe<T>(Action<T> handler) where T : IMessage
        {
            var messageType = typeof(T);
            if (!_handlers.TryGetValue(messageType, out var list))
            {
                list = new List<WeakReference<Action<IMessage>>>();
                _handlers[messageType] = list;
            }

            // Wrap en Action<IMessage> para uniformidad
            Action<IMessage> wrapped = msg => handler((T)msg);

            list.Add(new WeakReference<Action<IMessage>>(wrapped));
        }

        public void Unsubscribe<T>(Action<T> handler) where T : IMessage
        {
            var messageType = typeof(T);
            if (_handlers.TryGetValue(messageType, out var list))
            {
                // Buscar y remover el wrapped
                Action<IMessage> wrapped = msg => handler((T)msg);

                list.RemoveAll(wr =>
                {
                    if (wr.TryGetTarget(out var target))
                        return target == wrapped;
                    return true;  // Limpia refs muertas
                });
            }
        }

        public void Send<T>(T message) where T : IMessage
        {
            var messageType = typeof(T);
            if (_handlers.TryGetValue(messageType, out var list))
            {
                // Copia para evitar modifs durante iteración
                var copy = list.ToArray();
                foreach (var wr in copy)
                {
                    if (wr.TryGetTarget(out var handler))
                    {
                        handler(message);
                    }
                    else
                    {
                        list.Remove(wr);  // Limpia refs muertas
                    }
                }
            }
        }
    }
}