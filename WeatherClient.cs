using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace DotnetMicro
{
  class WeatherClient
  {
    private readonly HttpClient httpClient;
    private readonly ServiceSettings settings;
    public WeatherClient(HttpClient httpClient, IOptions<ServiceSettings> options)
    {
      this.httpClient = httpClient;
      settings = options.Value;
    }

    public record Weather(string description);
    public record Main(decimal temp);
    public record Forecast(Weather[] Weathers, Main Main, long dt);

    public async Task<Forecast> GetCurrentWeatherAsync(string city)
    {
      var uri = $"https://{settings.OpenWeatherHost}/data/2.5/weather?q={city}&appid={settings.ApiKey}&units=metric";
      var forecast = await httpClient.GetFromJsonAsync<Forecast>(uri);
      return forecast;
    }
  }
}