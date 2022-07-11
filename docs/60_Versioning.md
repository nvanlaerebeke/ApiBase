# Versioning

An important part of any API is versioning.  
API's created with the `ApiBase` project are required to implement at least some versioning, even if there is only one version at the start.  

## Startup

When the API is being configured there is already one abstract method that needs to be implemented:

```c#
protected override ApiVersion DefaultVersion() {
    return new ApiVersion(1, 0);
}
```

This will set the default version to use when no version is provided.  

## Requesting a Version

When the user does a REST call the API version is determent by the API version passed in either the header or as query string parameter.

In the header `X-API-VERSION` is used as key and for the query string parameter `api-version`.  

An example curl request:

```console
curl -X GET "http(s)://<hostname>/WeatherForecast?api-version=2.1" -H  "X-API-VERSION: 2.1"
```

When nothing is provided the default version configured in the startup is used.

## Implementation

There are 2 methods to specify a version:

- Controller wide
- Per method

It is recommended to use the Controller method, this makes for cleaner and more readable code.  

### Method Versioning

In the example below the controller provides two versions:

```c#
[ApiVersion("1.1")]
[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ApiController {
    public WeatherForecastController(IMapper mapper) : base(mapper) { }

    [HttpGet, ApiVersion("1.1")]
    public async Task<IActionResult> GetAll_v2([FromQuery] PaginationQuery paginationQuery) {
        var pagination = Mapper.Map<PaginationFilter>(paginationQuery);
        var forecasts = await Forecasts.GetForecasts(pagination);
        var result = Mapper.Map<List<ForecastResponse>>(forecasts);
        return Ok(new PageResponse<ForecastResponse>(result, pagination));
    }

    [ApiVersion("1.0")]
    [HttpGet]
    public async Task<IActionResult> GetAll_v1() {
        var forecasts = await Forecasts.GetForecasts();
        var result = Mapper.Map<List<ForecastResponse>>(forecasts);
        return Ok(result);
    }
}
```

The annotations for the controller itself needs a list of API versions this controller provides.  
This can be done by using the `ApiVersion` attribute:

```c#
[ApiVersion("1.1")]
[ApiVersion("1.0")]
public class WeatherForecastController : ApiController {
...
```

The next step is adding methods that provide the implementation for these versions, an example taken from the Sample API:

```c#
[HttpGet, ApiVersion("1.1")]
public async Task<IActionResult> GetAll_v2([FromQuery] PaginationQuery paginationQuery) {
    var pagination = Mapper.Map<PaginationFilter>(paginationQuery);
    var forecasts = await Forecasts.GetForecasts(pagination);
    var result = Mapper.Map<List<ForecastResponse>>(forecasts);
    return Ok(new PageResponse<ForecastResponse>(result, pagination));
}

[ApiVersion("1.0")]
[HttpGet]
public async Task<IActionResult> GetAll_v1() {
    var forecasts = await Forecasts.GetForecasts();
    var result = Mapper.Map<List<ForecastResponse>>(forecasts);
    return Ok(result);
}
```

Adding the `ApiVersion` attribute to the method denotes what version the method relates to.  
Note that there are multiple way's to do this:

```c#
//With the route & http method
[HttpGet, ApiVersion("1.1")]
//Stand-alone attribute
[ApiVersion("1.0")]
```

This method is not recommend as the controllers will get polluted fast and become unreadable. 

## Controller Versioning

Doing a version per controller is the recommended way of adding versioning support to the API's.  
Take the following two classes:

```c#
[ApiVersion("0.1", Deprecated = true)]
[ApiVersion("1.0")]
[ApiController]
[Route("WeatherForecast")]
public class WeatherForecastController_V1 : APIController
{
    [HttpGet("{id}")]
    public virtual IActionResult Get(string id){
        ...
    }
}
```

```c#
[ApiVersion("2.0")]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : WeatherForecastController_V1
{
    [HttpGet("{id}")]
    public override IActionResult Get(string id) {
        ...
    }
```

In the first (original) version of the controller the version `0.1` was deprecated and cannot be used anymore.  
The implementation for the controller is version `1.0`.
  
When a version `2.0` was released a new controller that inherits from the first controller was created.  

A few things had to be done to get this working:

- Rename the original controller to a `_V1` 
- Change the `Route` to `WeatherForecast` because the `[controller]` would result in a `_V1` if not set manually
- Create a new controller that inherits from the `V1`
