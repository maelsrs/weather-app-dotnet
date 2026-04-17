# WeatherApp

Application meteo en C# .NET utilisant Uno Platform et l'API OpenWeatherMap.

## Prerequis

- [.NET SDK 10.0+](https://dotnet.microsoft.com/download)
- Templates Uno Platform

## Installation

```bash
# Installer les templates Uno
dotnet new install Uno.Templates

# Restaurer les dependances
dotnet restore
```

## Lancer l'application

```bash
dotnet run
```

## Build

```bash
dotnet build
```

## Structure du projet

```
weather-app-csharp/
├── App.xaml / App.xaml.cs         # Point d'entree de l'app
├── MainPage.xaml                  # Interface (3 onglets)
├── MainPage.xaml.cs               # Logique des onglets
├── Models/                        # Classes de donnees
│   ├── WeatherData.cs             # Meteo actuelle
│   ├── ForecastData.cs            # Previsions
│   └── AppOptions.cs              # Parametres de l'app
├── Services/
│   ├── WeatherService.cs          # Appels API OpenWeatherMap
│   └── SettingsService.cs         # Lecture/ecriture options.json
├── Platforms/Desktop/Program.cs   # Main() desktop
├── Assets/                        # Icones et splash screen
└── WeatherApp.csproj              # Configuration du projet
```

## Parametres

Le fichier `options.json` est cree automatiquement au premier lancement dans :

Il contient la cle API OpenWeatherMap, la ville par defaut et la langue.
