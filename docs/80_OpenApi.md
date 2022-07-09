# OpenApi

The `OpenApi` also known as `swagger` documentation is automatically generated based on the `ApiControllers`.  

Adding the HTTP return codes to the documentation can be done as follows:

```c#
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
class MyApi : APIController {

}
```

The annotations can also be set on each method separately.

## Adding end-point Descriptions

The swagger documentation will display the XML comments in the source code when they're being exported.  

To enable this, open the project file of the project that contains the API controllers and and the following:

```xml
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <DocumentationFile>API.xml</DocumentationFile>
  </PropertyGroup>

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <DocumentationFile>API.xml</DocumentationFile>
  </PropertyGroup>

  <ItemGroup>
    <None Update="API.xml">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
```

This will generate the API.xml file and place it in the output directory.  
It is possible to change the `API.xml` filename by overriding the `get` method in the startup.  

Example:

```c#
public class Startup : ServerAPI.ServerAPI {
    ...
    protected override string GetAPIXmlDecriptionsFileName() {
        return "API.xml";
    }
    ...
}
```

## Controller Tag

Each controller is by default it's own tag in the swagger documentation, sometimes it's desirable to give your own name.  
This can be done by setting the `DisplayName` attribute on the class.  

```c#
[Route("[controller]")]
[DisplayName("Your New Name")]
public class WeatherForecastController : APIController {
}
```

If the `DisplayName` attribute is not sufficient custom logic can be added by overriding the `SetTags` method in the ServerAPI startup class.  

```c#
public class Startup : ServerAPI.ServerAPI {
    ...
    protected override void SetTags(SwaggerGenOptions options) {
        options.TagActionsBy(apiDesc => {
            if (apiDesc.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor) {
                var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(controllerActionDescriptor.ControllerTypeInfo, typeof(DisplayNameAttribute));
                if (attr != null) {
                    return new List<string> { attr.DisplayName };
                }
            }
            return new List<string> { apiDesc.ActionDescriptor.RouteValues["controller"] };
        });
    }
    ...
}
```
