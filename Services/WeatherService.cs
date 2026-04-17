using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class WeatherService
{
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5";
    private static readonly HttpClient Http = new HttpClient();

    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public async Task<WeatherData?> GetCurrentAsync(string city, string apiKey, string lang)
    {
        string url = BaseUrl + "/weather?q=" + Uri.EscapeDataString(city)
                   + "&appid=" + apiKey + "&units=metric&lang=" + lang;

        HttpResponseMessage response = await Http.GetAsync(url);
        await EnsureApiOk(response, city);

        return await response.Content.ReadFromJsonAsync<WeatherData>(JsonOptions);
    }

    public async Task<ForecastData?> GetForecastAsync(string city, string apiKey, string lang)
    {
        string url = BaseUrl + "/forecast?q=" + Uri.EscapeDataString(city)
                   + "&appid=" + apiKey + "&units=metric&lang=" + lang;

        HttpResponseMessage response = await Http.GetAsync(url);
        await EnsureApiOk(response, city);

        return await response.Content.ReadFromJsonAsync<ForecastData>(JsonOptions);
    }

    private static async Task EnsureApiOk(HttpResponseMessage response, string city)
    {
        if (response.IsSuccessStatusCode) return;

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            throw new CityNotFoundException("Ville '" + city + "' introuvable.");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new InvalidApiKeyException("Cle API invalide.");

        string body = await response.Content.ReadAsStringAsync();
        throw new WeatherApiException("Erreur API (" + (int)response.StatusCode + ") : " + body);
    }

    public static string GetIconUrl(string iconCode)
    {
        return "https://openweathermap.org/img/wn/" + iconCode + "@2x.png";
    }
}

public class CityNotFoundException : Exception
{
    public CityNotFoundException(string message) : base(message) { }
}

public class InvalidApiKeyException : Exception
{
    public InvalidApiKeyException(string message) : base(message) { }
}

public class WeatherApiException : Exception
{
    public WeatherApiException(string message) : base(message) { }
}
