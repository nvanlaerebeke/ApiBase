# Startup

Example:

```c#
public class Startup : ApiBase.ApiBase {
    public Startup(IConfiguration configuration) : base(configuration) { }

    protected override void ConfigureApi(IServiceCollection services) {
        _ = services.AddAuthentication("ApiKey").AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
        _ = services.AddSingleton<IApiKeyAuthentication>(x => new ApiKeyAuthentication(new ApiKeyProvider()));
        _ = services.AddAutoMapper(typeof(Startup));
    }

    protected override bool ConfigureOpenAPI(IServiceCollection services) {
        OpenAPIConfiguration.Setup(services);
        return true;
    }

    protected override ApiVersion DefaultVersion() {
        return new ApiVersion(1, 0);
    }

    protected override string GetName() {
        return "Sample API";
    }

    protected override IBadRequestErrorCodeProvider GetBadRequestErrorProvider(){
        return new BadRequestErrorCodeProvider();
    }
}
```

The base ApiBase class already does most of the heavy lifting for you.  
This class basically encapsulates your application so it can be run as an `ASP.NET Core Web API`.  

From a user's perspective some information needs to be provided, for example:

- What `Authentication` to enable/provide
- OpenAPI documentation configuration
- What version to use by default
- The application name
- A class that implements IBadRequestErrorCodeProvider to provide error codes (see [ErrorHandling](ErrorHandling.md))

## OpenAPI

Objects and versions are added automatically to the swagger documentation, what is not added is the authentication configuration.  
An example on how to add that to the docs is:

```c#
c.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
{
    Description = "Api key token validation",
    Name = "X-API-KEY",
    Type = SecuritySchemeType.ApiKey,
    In = ParameterLocation.Header
});
c.AddSecurityRequirement(new OpenApiSecurityRequirement() { {
    new OpenApiSecurityScheme {
        Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "X-API-KEY"
        },
        Scheme = "X-API-KEY",
        Name = "X-API-KEY",
        In = ParameterLocation.Header
    },
    new List<string>()
} });
```

There are a lot more configuration parameters for the OpenAPI integration, documentation is available at the Microsoft website.

## Base Path

It is not always desirable to run the endpoints on `/`, to solve this an configuration parameter can be passed during the configuration to change this.


Example:

```c#
public class Startup : ApiBase.ApiBase {
    protected override void ConfigureApi(IServiceCollection services) {
        ...
        Configuration["PathBase"] = "/mypath";
        ...
    }
}
```

Start the API in question on the foreground, the web endpoints will now be reachable also on:

```c#
/<basepath>/<controller>/<action>/...
```
