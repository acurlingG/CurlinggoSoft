using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CURLINGgo.Mobile.Services;

namespace CURLINGgo.Mobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty]
    private string _usuario = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _mensajeError = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
        {
            MensajeError = "Ingrese usuario y contraseña";
            return;
        }

        IsBusy = true;
        MensajeError = string.Empty;

        bool exito = await _authService.LoginAsync(Usuario, Password);

        IsBusy = false;

        if (exito)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            MensajeError = "Credenciales incorrectas o error de conexión";
        }
    }
}