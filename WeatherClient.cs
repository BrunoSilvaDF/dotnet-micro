using System.Net.Http;
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
  }
}