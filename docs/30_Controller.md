# Controller

The controllers are the entry-points for the endpoints, a small sample class:

```c#
[Route("[controller]")]
public class WeatherForecastController : APIController
{
    protected readonly IMapper Mapper;
    protected readonly WeatherForecasts Forecasts;

    public WeatherForecastController(IMapper mapper)
    {
        Mapper = mapper;
        Forecasts = new WeatherForecasts();
    }

    [HttpGet("{id}")]
    public virtual IActionResult Get(string id)
    {
        var foreCast = Forecasts.Get(id);
        if (foreCast == null)
        {
            return NotFound();
        }
        return Ok(foreCast);
    }
}
```

Each controller must extend `APIController` and have a constructor with the IMapper as constructor.  
The constructor can differ based on the configured services, by default `ServerAPI` adds the mapper for separation between Data and API objects.  

In the controller the REST methods are configured by adding the `HTTP method` with the path parameters.  

The CRUD actions are as follows:

- HttpGet
- HttpPost
- HttpPut
- HttpDelete

Example:

```c#
[HttpGet("{id}")]
public override IActionResult Get(...) 

[HttpGet]
public async Task<IActionResult> GetAll(...)

[HttpPost]
public IActionResult Create(...)

[HttpPut]
public IActionResult Update(...)

[HttpDelete("{id}")]
public IActionResult Delete(...)
```

## API Return Value

The return is always an IActionResult, DotNet has a few shortcut methods for this:

- Ok(): HTTP 200
- NotFound(): HTTP 404
- BadRequest(): HTTP 400


Instead of returning the object directly each response should be one of the following classes:
- Response
- PageResponse
- Redirect(new RedirectResponse<ReturnType>(APIController, Object).Url.ToString())

The reason is for a uniform return format that still gives flexibility for future changes.  

## Response

This is the most simple return class and wraps whatever object being returned in a `data { }` property.  

Take the following code:

```c#
return Ok(
  new Response<Forecast>(
    new Forecast() {
      ID = "bf5bc539-becf-408f-83fd-285000692124",
      Date = DateTime.Now(),
      TemperatureC = 8,
      Summary = "Chilly"
    }
  )
);
```

This would result in the following JSON return:

```json
{
  "data": {
      "id": "bf5bc539-becf-408f-83fd-285000692124",
      "date": "2021-04-14T06:46:08.0139545+00:00",
      "temperatureC": 8,
      "temperatureF": 46,
      "summary": "Chilly"
    }
}
```

## RedirectResponse

When doing `[POST]` (create) and `[PUT]` (update) requests the response is almost always a "`HTTP 302 [Redirect]`" with in the response headers the URL to the resource.

An example:

```c#
return Redirect(new RedirectResponse<ForcastResponse>(this, new ForecastResponse() { ID = "bf5bc539-becf-408f-83fd-285000692124"}).Url.ToString());
```

This would set the `Location` header to:

```text
<schema>://<hostname>/WeatherForecast/bf5bc539-becf-408f-83fd-285000692124
```

## PageResponse

Doing a `[GET]` request that returns a `List<object>` often implements pagination, the response should contain what page is represented with it's offset and the next and previous pages if there are any.  

To do this:

```c#
return Ok(new PageResponse<ForecastResponse>(result, pagination));
```

This would have a `JSON` result like:

```json
{
  "data": [
    {
      ...
    },
    {
      ...
    }
  ],
  "page": 1,
  "size": 2,
  "nextPage": 2,
  "previousPage": 1
}
```
