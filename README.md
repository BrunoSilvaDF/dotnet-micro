Microservice in .NET 5, that comunicates with https://openweathermap.org/current

## Tips/Commands

`dotnet new webapi -o DotnetMicro --no-https` create project with no https

`dotnet user-secrets init` init SecretManager

`dotnet user-secrets set ServiceSettings:ApiKey {APIKEY}` set ApiKey using SecretManager
