using System.Net.Http;
using Microsoft.UI.Xaml.Media.Imaging;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp;

public sealed partial class MainPage : Page
{
    private WeatherService _weather = new WeatherService();
    private AppOptions _options;

    public MainPage()
    {
        this.InitializeComponent();
        _options = SettingsService.Load();
        ApplyOptionsToUi();
        AutoFillDefaultCity();
    }

    private void ApplyOptionsToUi()
    {
        ApiKeyInput.Text = _options.ApiKey;
        DefaultCityInput.Text = _options.DefaultCity;

        foreach (ComboBoxItem item in LanguageCombo.Items)
        {
            string tag = (string)item.Tag;
            if (tag == _options.Language)
            {
                LanguageCombo.SelectedItem = item;
                break;
            }
        }

        if (LanguageCombo.SelectedItem == null && LanguageCombo.Items.Count > 0)
            LanguageCombo.SelectedIndex = 0;
    }

    private void AutoFillDefaultCity()
    {
        if (!string.IsNullOrWhiteSpace(_options.DefaultCity))
        {
            SearchCityInput.Text = _options.DefaultCity;
            ForecastCityInput.Text = _options.DefaultCity;
        }
    }

    private async void OnSearchClick(object sender, RoutedEventArgs e)
    {
        SearchError.Visibility = Visibility.Collapsed;
        SearchResult.Visibility = Visibility.Collapsed;

        string city = SearchCityInput.Text.Trim();
        if (city.Length == 0)
        {
            ShowSearchError("Veuillez entrer une ville.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            ShowSearchError("Cle API manquante. Renseignez-la dans les parametres.");
            return;
        }

        try
        {
            WeatherData? data = await _weather.GetCurrentAsync(city, _options.ApiKey, _options.Language);
            if (data == null)
            {
                ShowSearchError("Reponse vide.");
                return;
            }

            ResCity.Text = data.Name;
            ResCoords.Text = "Latitude : " + data.Coord.Lat + "   Longitude : " + data.Coord.Lon;
            ResTemp.Text = "Temperature : " + data.Main.Temp.ToString("0.#") + " C";
            ResHumidity.Text = "Humidite : " + data.Main.Humidity + "%";

            if (data.Weather.Count > 0)
            {
                ResDesc.Text = "Temps : " + data.Weather[0].Description;
                ResIcon.Source = new BitmapImage(new Uri(WeatherService.GetIconUrl(data.Weather[0].Icon)));
            }
            else
            {
                ResDesc.Text = "";
            }

            SearchResult.Visibility = Visibility.Visible;
        }
        catch (CityNotFoundException ex) { ShowSearchError(ex.Message); }
        catch (InvalidApiKeyException ex) { ShowSearchError(ex.Message); }
        catch (WeatherApiException ex) { ShowSearchError(ex.Message); }
        catch (HttpRequestException) { ShowSearchError("Pas de connexion internet."); }
        catch (Exception ex) { ShowSearchError("Erreur : " + ex.Message); }
    }

    private void ShowSearchError(string msg)
    {
        SearchError.Text = msg;
        SearchError.Visibility = Visibility.Visible;
    }

    private async void OnForecastClick(object sender, RoutedEventArgs e)
    {
        ForecastError.Visibility = Visibility.Collapsed;
        ForecastList.ItemsSource = null;

        string city = ForecastCityInput.Text.Trim();
        if (city.Length == 0)
        {
            ShowForecastError("Veuillez entrer une ville.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            ShowForecastError("Cle API manquante. Renseignez-la dans les parametres.");
            return;
        }

        try
        {
            ForecastData? data = await _weather.GetForecastAsync(city, _options.ApiKey, _options.Language);
            if (data == null)
            {
                ShowForecastError("Reponse vide.");
                return;
            }

            List<ForecastCardVm> cards = new List<ForecastCardVm>();

            foreach (ForecastItem item in data.List)
            {
                if (!item.DtTxt.EndsWith("12:00:00")) continue;
                if (cards.Count >= 5) break;

                ForecastCardVm card = new ForecastCardVm();
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(item.Dt).LocalDateTime;
                card.DateLabel = date.ToString("dd/MM/yyyy HH:mm");
                card.CityName = data.City.Name;
                card.Coords = "Lat : " + data.City.Coord.Lat + " Lon : " + data.City.Coord.Lon;
                card.Temp = item.Main.Temp.ToString("0.#") + " C";
                card.Humidity = "Humidite : " + item.Main.Humidity + "%";

                if (item.Weather.Count > 0)
                {
                    card.Description = item.Weather[0].Description;
                    card.IconUrl = WeatherService.GetIconUrl(item.Weather[0].Icon);
                }

                cards.Add(card);
            }

            ForecastList.ItemsSource = cards;
        }
        catch (CityNotFoundException ex) { ShowForecastError(ex.Message); }
        catch (InvalidApiKeyException ex) { ShowForecastError(ex.Message); }
        catch (WeatherApiException ex) { ShowForecastError(ex.Message); }
        catch (HttpRequestException) { ShowForecastError("Pas de connexion internet."); }
        catch (Exception ex) { ShowForecastError("Erreur : " + ex.Message); }
    }

    private void ShowForecastError(string msg)
    {
        ForecastError.Text = msg;
        ForecastError.Visibility = Visibility.Visible;
    }

    private void OnSaveSettingsClick(object sender, RoutedEventArgs e)
    {
        _options.ApiKey = ApiKeyInput.Text.Trim();
        _options.DefaultCity = DefaultCityInput.Text.Trim();

        ComboBoxItem? selected = LanguageCombo.SelectedItem as ComboBoxItem;
        if (selected != null)
            _options.Language = (string)selected.Tag;

        SettingsService.Save(_options);
        SettingsStatus.Text = "Parametres enregistres.";
        AutoFillDefaultCity();
    }
}

public class ForecastCardVm
{
    public string DateLabel { get; set; } = "";
    public string CityName { get; set; } = "";
    public string Coords { get; set; } = "";
    public string Temp { get; set; } = "";
    public string Description { get; set; } = "";
    public string Humidity { get; set; } = "";
    public string IconUrl { get; set; } = "";
}
