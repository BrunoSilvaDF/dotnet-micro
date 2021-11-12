using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotnetMicro.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherClient weatherClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherClient weatherClient)
    {
      _logger = logger;
      this.weatherClient = weatherClient;
    }

    [HttpGet]
    [Route("{city}")]
    public async Task<WeatherForecast> Get(string city)
    {
      var forecast = await weatherClient.GetCurrentWeatherAsync(city);
      return new WeatherForecast
      {
        Summary = forecast.Weather[0].description,
        TemperatureC = (int)forecast.Main.temp,
        Date = DateTimeOffset.FromUnixTimeSeconds(forecast.dt).DateTime
      };
    }
  }
}
