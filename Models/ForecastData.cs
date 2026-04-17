namespace WeatherApp.Models;

public class ForecastData
{
    public List<ForecastItem> List { get; set; } = new List<ForecastItem>();
    public City City { get; set; } = new City();
}

public class ForecastItem
{
    public long Dt { get; set; }
    public string DtTxt { get; set; } = "";
    public Main Main { get; set; } = new Main();
    public List<Weather> Weather { get; set; } = new List<Weather>();
}

public class City
{
    public string Name { get; set; } = "";
    public Coord Coord { get; set; } = new Coord();
}
