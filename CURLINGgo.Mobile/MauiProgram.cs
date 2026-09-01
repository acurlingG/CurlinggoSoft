using CURLINGgo.Mobile.Services;
using CURLINGgo.Mobile.ViewModels;
using CURLINGgo.Mobile.Views;

namespace CURLINGgo.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servicios HTTP y Auth
        builder.Services.AddHttpClient<AuthService>();
        builder.Services.AddSingleton<AuthService>();

        // ViewModels y Vistas
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        return builder.Build();
    }
}