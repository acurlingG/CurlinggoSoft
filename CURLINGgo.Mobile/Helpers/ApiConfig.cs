namespace CURLINGgo.Mobile.Helpers;

public static class ApiConfig
{
    // Reemplaza 5123 por el puerto HTTP real de tu API
    private const string Puerto = "5264";

    public static string BaseUrl = DeviceInfo.Platform == DevicePlatform.Android
        ? $"http://10.0.2.2:{Puerto}/api/"
        : $"http://localhost:{Puerto}/api/";
}