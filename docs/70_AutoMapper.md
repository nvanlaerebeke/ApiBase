# Mapping Objects

There is a difference between the data object and the API contract.  
The API contract being the in and output of the REST API.  

A model/entity for example should never be used as response object.  
  
To make this process easier `AutoMapper` was used.  
If 70% or more of two objects match it properties it is best to automate the process of converting one into the other.  
An example of the `WeatherForecast` classes in the Sample API.

The API contacts class:

```c#
public class ForecastResponse : IAPIObject {
    public string ID { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [SummaryValidation]
    [Required]
    public string Summary { get; set; }
}
```

The data class (in practice an entity f.e.):

```c#
public class Forecast {
    public string ID { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string Summary { get; set; }
}
```

As you can see the two objects do different things but have mostly the same properties.  
The data object can be an Entity and doesn't do any input validation, while the API object has all the input validation and an extra calculated property.  

To make the conversion between the two automatic create a `AutoMapper.Profile` class:

```c#
public class ResponseToDomainProfile : Profile {
    public ResponseToDomainProfile() {
        CreateMap<Forecast, ForecastResponse>();
        CreateMap<ForecastResponse, Forecast>();
    }
}
```

This makes it so that the mapper can convert `to` and `from` between the `Forecast` classes.  
Loading the mapper is done in the startup by adding the following line:

```c#
protected override void ConfigureApi(IServiceCollection services) {
    services.AddAutoMapper(typeof(Startup));
}
```

The mapper is passed in the constructor of the controllers:

```c#
private readonly IMapper Mapper
public WeatherForecastController(IMapper mapper) {
    Mapper = mapper;
}
```

And can be used like this:

```c#
var foreCast = new Forecast();
var response = Mapper.Map<ForecastResponse>(foreCast);
```

More details about `AutoMapper` can be found on the [AutoMapper website](https://docs.automapper.org)