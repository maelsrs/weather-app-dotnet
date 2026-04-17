using System.IO;
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services;

public static class SettingsService
{
    private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "WeatherApp",
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
        AppOptions? options = JsonSerializer.Deserialize<AppOptions>(json);
        if (options == null) return new AppOptions();
        return options;
    }

    public static void Save(AppOptions options)
    {
        string? dir = Path.GetDirectoryName(FilePath);
        if (dir != null && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        JsonSerializerOptions opts = new JsonSerializerOptions();
        opts.WriteIndented = true;

        string json = JsonSerializer.Serialize(options, opts);
        File.WriteAllText(FilePath, json);
    }
}
