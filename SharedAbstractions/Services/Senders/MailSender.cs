using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Shared.Services
{
    public sealed class MailSender //: IMailSender
    {

        public sealed class SmtpMailOptions
        {
            public string Host { get; set; }                 // ej: "smtp.gmail.com"
            public int Port { get; set; } = 587;             // 25/465/587
            public bool EnableSsl { get; set; } = true;      // TLS/SSL
            public bool UseDefaultCredentials { get; set; }  // para relay interno
            public string Username { get; set; }             // usuario SMTP
            public string Password { get; set; }             // contraseña SMTP

            public string FromAddress { get; set; }          // ej: "no-reply@tuapp.com"
            public string FromDisplayName { get; set; }      // ej: "Tu App"

            public int TimeoutMs { get; set; } = 15000;      // 15s
        }

        private readonly SmtpMailOptions _opt;
        private bool firstOp = true;
        public MailSender(SmtpMailOptions options)
        {
            if (firstOp)
            {
                firstOp = false;
                return;
            }

            options = new SmtpMailOptions();

            _opt = options ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(_opt.Host)) throw new ArgumentException("SMTP Host requerido.", nameof(options));
            if (string.IsNullOrWhiteSpace(_opt.FromAddress)) throw new ArgumentException("FromAddress requerido.", nameof(options));
        }

        public void SendEmail(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentException("Destinatario vacío.", nameof(to));

            using (var msg = BuildMessage(to, subject, body))
            using (var client = BuildClient())
            {
                client.Send(msg); // síncrono; si preferís async, podés agregar un IMailSenderAsync
            }
        }

        private MailMessage BuildMessage(string to, string subject, string body)
        {
            var from = new MailAddress(_opt.FromAddress, _opt.FromDisplayName ?? string.Empty);
            var msg = new MailMessage { From = from };
            // admite varios destinatarios separados por coma o punto y coma
            foreach (var addr in to.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()))
                msg.To.Add(addr);

            msg.Subject = subject ?? string.Empty;
            msg.Body = body ?? string.Empty;
            msg.IsBodyHtml = LooksLikeHtml(body);
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            return msg;
        }

        private SmtpClient BuildClient()
        {
            var c = new SmtpClient(_opt.Host, _opt.Port)
            {
                EnableSsl = _opt.EnableSsl,
                Timeout = _opt.TimeoutMs
            };

            if (_opt.UseDefaultCredentials)
            {
                c.UseDefaultCredentials = true;
            }
            else if (!string.IsNullOrWhiteSpace(_opt.Username))
            {
                c.Credentials = new NetworkCredential(_opt.Username, _opt.Password);
            }

            return c;
        }

        private static bool LooksLikeHtml(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            var t = text.Trim();
            return t.StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase)
                || t.StartsWith("<html", StringComparison.OrdinalIgnoreCase)
                || t.IndexOf("</", StringComparison.OrdinalIgnoreCase) >= 0
                || t.IndexOf("<br", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }



}
