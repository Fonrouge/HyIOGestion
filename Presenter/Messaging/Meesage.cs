using System;

namespace Presenter.Messaging  // Ajusta a tu namespace en Shared
{
    /// <summary>
    /// Clase base abstracta para todos los mensajes en el sistema de mensajería.
    /// Hereda de esta para crear mensajes específicos, implementando el Payload si es necesario.
    /// </summary>
    public abstract class Message: IMessage
    {
        /// <summary>
        /// Remitente del mensaje (opcional, para tracing o filtros).
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Timestamp de cuando se creó el mensaje (automático).
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Constructor base.
        /// </summary>
        /// <param name="sender">Objeto que envía el mensaje (opcional).</param>
        protected Message(object sender = null)
        {
            Sender = sender;
        }

        /// <summary>
        /// Método virtual para obtener una descripción del mensaje (para logging o debug).
        /// Sobrescribir en hijos si querés detalles específicos.
        /// </summary>
        /// <returns>Descripción del mensaje.</returns>
        public virtual string GetDescription()
        {
            return $"Mensaje de tipo {GetType().Name} enviado por {Sender?.GetType().Name ?? "Anónimo"} a las {Timestamp}";
        }
    }

    /// <summary>
    /// Versión genérica de Message para mensajes con payload (datos específicos).
    /// Usa esta para la mayoría de casos.
    /// </summary>
    /// <typeparam name="T">Tipo del payload (ej: string, int, objeto custom).</typeparam>
    public abstract class Message<T> : Message
    {
        /// <summary>
        /// Datos asociados al mensaje (payload).
        /// </summary>
        public T Payload { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="payload">Datos del mensaje.</param>
        /// <param name="sender">Remitente (opcional).</param>
        protected Message(T payload, object sender = null) : base(sender)
        {
            Payload = payload;
        }

        public override string GetDescription()
        {
            return base.GetDescription() + $" con payload de tipo {typeof(T).Name}";
        }
    }
}