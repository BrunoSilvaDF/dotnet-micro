Microservice in .NET 5, that consumes https://openweathermap.org/current

Based on https://www.youtube.com/watch?v=MIJJCR3ndQQ

## Tips/Commands

`dotnet new webapi -o DotnetMicro --no-https` create project with no https

`dotnet user-secrets init` init SecretManager

`dotnet user-secrets set ServiceSettings:ApiKey {APIKEY}` set ApiKey using SecretManager

`dotnet add package Microsoft.Extensions.Http.Polly` add pkg to handle http requests
