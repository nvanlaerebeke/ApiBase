# API Key Authentication and Authorization

The ServerAPI has API Key authentication build in, there are two steps to the process, the Authentication and Authorization.

Before the build in authentication can be used it must be enabled and configured.

## Configuration

In the startup, add the following:

```c#
protected override void ConfigureApi(IServiceCollection services)
{
    services
        .AddAuthentication("ApiKey")
        .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
    services
        .AddSingleton<IApiKeyAuthentication>(x => new ApiKeyAuthentication(new ApiKeyProvider()));
}
```

This enables the API Key authentication and sets the class that will provide the API Keys.  

## API Key Provider

The keys must be provided by someone, this can be a database or a hardcoded list.  
A class that implements `IApiKeyProvider` as to be set as the provider in the startup.

This class has to implement a method `GetByKey` that returns `IApiKeyAuthenticationInfo` that will be used in the authentication and authorization process

## Example Request

```console
curl -X GET "http://localhost:49153/WeatherForecast" -H  "X-API-KEY: SomeKey"
```
