using System.Net.Http.Json;
using CURLINGgo.Mobile.Helpers;
using CURLINGgo.Mobile.Models;

namespace CURLINGgo.Mobile.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private const string TokenKey = "auth_token";

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(ApiConfig.BaseUrl);
    }

    public async Task<bool> LoginAsync(string usuario, string password)
    {
        try
        {
            var request = new LoginRequest { Usuario = usuario, Password = password };
            var response = await _httpClient.PostAsJsonAsync("cuentas/login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await SecureStorage.SetAsync(TokenKey, result.Token);
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> GetTokenAsync() => await SecureStorage.GetAsync(TokenKey);

    public void Logout() => SecureStorage.Remove(TokenKey);
}