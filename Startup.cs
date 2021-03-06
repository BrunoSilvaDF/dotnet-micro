using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;

namespace DotnetMicro
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // inject ServiceSettings
      // can access with IOptions<ServiceSettings> interface
      services.Configure<ServiceSettings>(Configuration.GetSection(nameof(ServiceSettings)));

      // inject HttpClient into WeatherClient
      services.AddHttpClient<WeatherClient>()
      // add Polly to handle remote server requests
        .AddTransientHttpErrorPolicy(builder =>
          builder.WaitAndRetryAsync(
            // retry 10 times
            10,
            // on each retry attempt, will sleep as much as this
            retryAttempt =>
              // exponential backoff
              // will await 2^retry attempt
              TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
          )
        )
        // Circuit Break Policy
        // will allow to try 3 times, if keep failing, break
        .AddTransientHttpErrorPolicy(builder =>
          builder.CircuitBreakerAsync(
            3,
            TimeSpan.FromSeconds(10)
          )
        );

      // add healthchecks
      services.AddHealthChecks().AddCheck<ExternalEndpointHealthCheck>("OpenWeather");

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotnetMicro", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetMicro v1"));
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();

        // add healthchecks endpoint
        endpoints.MapHealthChecks("/health");
      });
    }
  }
}
