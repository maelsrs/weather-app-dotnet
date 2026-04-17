namespace WeatherApp.Models;

public class WeatherData
{
    public string Name { get; set; } = "";
    public Coord Coord { get; set; } = new Coord();
    public Main Main { get; set; } = new Main();
    public List<Weather> Weather { get; set; } = new List<Weather>();
}

public class Coord
{
    public double Lon { get; set; }
    public double Lat { get; set; }
}

public class Main
{
    public double Temp { get; set; }
    public int Humidity { get; set; }
}

public class Weather
{
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
}
