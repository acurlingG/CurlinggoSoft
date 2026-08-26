using System.Net;
using System.Net.Mail;

namespace CurlinggoSoft.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendTwoFactorCodeAsync(string toEmail, string code)
        {
            var mail = ArmarMensajeBase(
                toEmail,
                "Tu código de verificación - CURLINGgo",
                $@"
                <h3>Código de verificación</h3>
                <p>Tu código para iniciar sesión en CURLINGgo es:</p>
                <p style=""font-size: 28px; font-weight: bold; letter-spacing: 4px;"">{code}</p>
                <p>Este código expira en unos minutos. Si no intentaste iniciar sesión, ignora este correo.</p>");

            await EnviarAsync(mail, "código de verificación 2FA", lanzarSiFalla: true);
        }

        public async Task SendLoginAlertAsync(string toEmail, string nombreUsuario, bool exitoso, string motivo)
        {
            var fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            var mail = exitoso
                ? ArmarMensajeBase(
                    toEmail,
                    "Se inició sesión en tu cuenta - CURLINGgo",
                    $@"
                    <h3>Inicio de sesión exitoso</h3>
                    <p>Hola {nombreUsuario},</p>
                    <p>Se inició sesión en tu cuenta de CURLINGgo el <strong>{fechaHora}</strong>.</p>
                    <p>Si fuiste tú, no necesitas hacer nada. Si no reconoces este acceso, cambia tu contraseña de inmediato.</p>")
                : ArmarMensajeBase(
                    toEmail,
                    "Intento de inicio de sesión en tu cuenta - CURLINGgo",
                    $@"
                    <h3>Intento de inicio de sesión fallido</h3>
                    <p>Hola {nombreUsuario},</p>
                    <p>Hubo un intento de inicio de sesión en tu cuenta de CURLINGgo el <strong>{fechaHora}</strong> que no se completó.</p>
                    <p><strong>Motivo:</strong> {motivo}</p>
                    <p>Si no fuiste tú, te recomendamos cambiar tu contraseña.</p>");

            // A diferencia del código 2FA, si falla el envío de esta alerta NO debe
            // bloquear el login del usuario (ya inició sesión o ya falló por su cuenta;
            // esto es solo una notificación informativa).
            await EnviarAsync(mail, "alerta de inicio de sesión", lanzarSiFalla: false);
        }

        private MailMessage ArmarMensajeBase(string toEmail, string asunto, string cuerpoHtml)
        {
            var smtpSettings = _config.GetSection("SmtpSettings");
            var username = smtpSettings["Username"];
            var fromEmail = smtpSettings["FromEmail"] ?? username;

            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail!, "CURLINGgo"),
                Subject = asunto,
                IsBodyHtml = true,
                Body = cuerpoHtml
            };
            mail.To.Add(toEmail);
            return mail;
        }

        private async Task EnviarAsync(MailMessage mail, string descripcion, bool lanzarSiFalla)
        {
            var smtpSettings = _config.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"] ?? "587");
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];

            try
            {
                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar {Descripcion}", descripcion);
                if (lanzarSiFalla)
                {
                    throw;
                }
            }
            finally
            {
                mail.Dispose();
            }
        }
    }
}