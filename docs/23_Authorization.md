# Authorization

Once authentication is done the next step in the pipeline is authorization.
Authorization can be done on a per `class` or `method` bases and expects an attribute that extends the `BaseAuthorization` class.  
  
In the example here an authorization class for validating the  `ApiKey` role will be shown.

```c#
internal class WeatherForecastGetAuthorization : ApiKeyAuthorization {
    protected override bool Authorize(IEnumerable<string> roles) {
        return roles.Contains("Controller");
    }
}
```

The `Authorize` method is called automatically with a list of roles, this is the API key `type` that is set in the `IApiKeyAuthenticationInfo` and can be used to allow (return true) or deny (return false) access.  

Returning false will case a `[HTTP] 403` response.

## Aditional Validation

Sometimes based on what parameters the user provides the access is granted or not.  
The `HttpContext` can be accessed using the `this.Context` property, there is also a build in method to get the querystring called `GetQuery()`.  

Example:

```c#
class GetQueryExample : ApiKeyAuthorization {
    protected override bool Authorize(IEnumerable<string> roles)
    {
        var query = GetQuery();
        var httpContext = this.Context;
        return (query.Count == 0);
    }
}
```
