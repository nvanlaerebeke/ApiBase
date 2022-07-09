# Customizing Authorization

## Authentication

Creating an authentication method is adding a `Handler` that extends `AuthenticationHandler<ApiKeyAuthenticationOptions>`.  
As an example the integrated API key authentication will be explained.  

It starts from the `Startup` class that implements the `ConfigureApi()` method, here the new schema must be added like this:

```c#
services.AddAuthentication("ApiKey").AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
services.AddSingleton<IApiKeyAuthentication>(x => new ApiKeyAuthentication(new ApiKeyProvider()));
```

The first line adds the options and class that will be used to do the actual authentication step on each request.  
The second line adds an IOC class that will provide the API keys to the authentication, this is because the source is different from app to app.  

For the options class the defaults are most of the time OK, this needs to be a class that extends `AuthenticationSchemeOptions`.  
The class that will do the actual validation for each request is the `ApiKeyAuthenticationHandler` that must extend `AuthenticationHandler<ApiKeyAuthenticationOptions>`.  

## Handler

An example of the class for API keys is:

```c#
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions> {
    private readonly IApiKeyAuthentication Authenticator;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IApiKeyAuthentication authenticator
    ) : base(options, logger, encoder, clock)
    {
        Authenticator = authenticator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
        return await Task.Run(() =>{
            ///Headers should be treated case insensitive (see HTTP RFC)
            var ApiKey = Request.Headers.Where(h => h.Key.ToLower().Equals("x-api-key")).Select(k => k.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(ApiKey))
            {
                return Validate(ApiKey);
            }
            return AuthenticateResult.Fail("Missing X-API-KEY header");
        });
    }

    private AuthenticateResult Validate(string apiKey){
        var key = Authenticator.Authenticate(new ApiKeyAuthenticationParams(apiKey));
        if (key == null)
        {
            return AuthenticateResult.Fail("API Key not found");
        }
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, key.Key)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new GenericPrincipal(identity, new string[] { key.Type });
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}
```

In the constructor both the `ApiKeyAuthenticationOptions` and the `IApiKeyAuthentication` class that was set in the Startup can be found.  
Once the authentication starts, the `HandleAuthenticateAsync()` method will be called, starting there your custom implementation can done.  

### Response

The response is always an IAuthenticateResult, .NET has shortcut methods to easily return this:

- AuthenticateResult.Success()
- AuthenticateResult.Fail()

As you can see in the sample the Success expects a ticket, for more information about what tickets, principles and roles are, the Microsoft docs contain plenty of information.
