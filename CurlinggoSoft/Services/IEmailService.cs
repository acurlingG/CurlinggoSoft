namespace CurlinggoSoft.Services
{
    public interface IEmailService
    {
        // Para mandar el código de verificación de dos factores al correo electrónico del usuario.
        Task SendTwoFactorCodeAsync(string toEmail, string code);

        // Para avisar por correo cada intento de inicio de sesión (exitoso o fallido),
        // sin importar el rol (Admin, Cliente o Tecnico).
        Task SendLoginAlertAsync(string toEmail, string nombreUsuario, bool exitoso, string motivo);
    }
}