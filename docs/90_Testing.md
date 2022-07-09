# Testing

Besides the normal Unit Tests that are expected to be created the startup methods and `Controllers`  an be tested by creating integration tests.

These tests spin up a webserver and do the actual requests.  

The included sample application contains an example of how to implement the integration tests.

## Project

Create a `ASP.net Web API` project to be used as the entry-point project for the integration tests.  

Remove the default content of the project and update the project `csproj` to something like:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
    <IsPackable>false</IsPackable>
    <SignAssembly>true</SignAssembly>
    <AssemblyOriginatorKeyFile>key.snk</AssemblyOriginatorKeyFile>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="5.0.4" />
    <PackageReference Include="NUnit" Version="3.12.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="3.16.1" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.5.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\CertManager\CertManager.csproj" />
  </ItemGroup>
</Project>
```

## Setup

Assuming the project `Sample.Tests` exists, add a `WebApplicationFactory` that will bootstrap your application.  

In case of the include sample:

```c#
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        //Can be used to for example initialize and pre-seed a DBContext
        //Set a base path to also make the api available here
        Environment.SetEnvironmentVariable("PATH_BASE", "/sample");
    }
}
```

## Controller Setup

Now it's time to create a class that will do the actual test.  
Add a reference to your main project (`Sample.csproj`)

```c#
class WeatherForecastGetTest {
    protected readonly HttpClient _client;

    public WeatherForecastGetTest(ApiVersion version) {
        var version new ApiVersion(1, 0);
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            }
        );
        _client.DefaultRequestHeaders.Add("X-API-KEY", "SomeKey");
        _client.DefaultRequestHeaders.Add("X-API-VERSION", version.ToString());
    }
}
```

The `Program` class passed in the `CustomWebapplicationFactory` is the `Sample.Program`, so the real entry-point.  

There are also two headers being added in this setup, these will be added in every request.  
Modify/remove these lines based on your own application.  

the `_client` is the `HttpClient` that will be used to do the actual requests.  

## Test

### GET

A simple `[GET]` example:

```c#
[Test]
public virtual async Task TestGet() {
    //Arrange
    // Act
    var response = await _client.GetAsync(Routes.WeatherForecast.Get("b2216615-de2e-457b-99c1-0376fddecc04")).ConfigureAwait(false);

    // Assert
    Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
    var responseObject = JsonToObject<ForecastResponse>(json);
    Assert.NotNull(responseObject);
}

protected T JsonToObject<T>(string json) {
    var options = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
    };
    return JsonSerializer.Deserialize<T>(json, options);
}
```

### POST

A post example:

```c#
[Test]
public async Task TestCreate() {
    //Arrange
    var obj = new ForecastResponse() {
        ID = Guid.NewGuid().ToString(),
        Date = DateTime.Now,
        Summary = nameof(Summary.Chilly),
        TemperatureC = 666
    };
    var json = JsonSerializer.Serialize(obj);
    var data = new StringContent(json, Encoding.UTF8, "application/json");

    // Act
    var response = await _client.PostAsync(Routes.WeatherForecast.Create(), data).ConfigureAwait(false);

    // Assert
    Assert.AreEqual(System.Net.HttpStatusCode.Redirect, response.StatusCode);
    Assert.That(response.Headers.Contains("Location"));
    Assert.IsTrue(response.Headers.TryGetValues("Location", out var location));
    Assert.AreNotEqual(new Uri(location.First()).LocalPath, "/WeatherForecast/" + obj.ID); //test if the ID provided was not used for the create
    Assert.That(new Uri(location.First()).LocalPath.Length.Equals(("/WeatherForecast/" + obj.ID).Length));
}
```
