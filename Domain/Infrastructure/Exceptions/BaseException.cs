using System;


namespace Domain.Exceptions.Base
{
    public class BaseException : Exception
    {
        public SeverityEnum Severity { get; }
        public DateTime Timestamp { get; set; }

        public BaseException(string message, SeverityEnum severity) : base(message)
        {
            Severity = severity;
            Timestamp = DateTime.Now;            
        }
        public BaseException(string message) : base(message)
        {
            Timestamp = DateTime.Now;
        }

    }

}
