using System.IO;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public static class SettingsService
{
    private static readonly string FilePath = Path.Combine(
        AppContext.BaseDirectory,
        "options.json");

    public static AppOptions Load()
    {
        if (!File.Exists(FilePath))
        {
            AppOptions defaults = new AppOptions();
            Save(defaults);
            return defaults;
        }

        string json = File.ReadAllText(FilePath);
        AppOptions? options = JsonConvert.DeserializeObject<AppOptions>(json);
        if (options == null) return new AppOptions();
        return options;
    }

    public static void Save(AppOptions options)
    {
        string? dir = Path.GetDirectoryName(FilePath);
        if (dir != null && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonConvert.SerializeObject(options, Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }
}
