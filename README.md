# WeatherApp

Application meteo en C# .NET utilisant Uno Platform et l'API OpenWeatherMap.

## Prerequis

- [.NET SDK 10.0+](https://dotnet.microsoft.com/download)
- Templates Uno Platform

## Installation

```bash
# Clone le repo
git clone https://github.com/maelsrs/weather-app-dotnet
cd weather-app-dotnet

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
weather-app-dotnet/
├── App.xaml / App.xaml.cs         # Point d'entrée de l'app
├── MainPage.xaml                  # Interface (3 onglets)
├── MainPage.xaml.cs               # Logique des onglets
├── Models/                        # Classes de données
│   ├── WeatherData.cs             # Météo actuelle
│   ├── ForecastData.cs            # Prévisions
│   └── AppOptions.cs              # Paramètres de l'app
├── Services/
│   ├── WeatherService.cs          # Appels API OpenWeatherMap
│   └── SettingsService.cs         # Lecture/écriture options.json
├── Platforms/Desktop/Program.cs   # Main() desktop
├── Assets/                        # Icones et splash screen
└── WeatherApp.csproj              # Configuration du projet
```

## Parametres

Le fichier `options.json` se crée automatiquement au premier lancement dans :

Il contient la clé API OpenWeatherMap, la ville par defaut et la langue.
