# ErrorHandler

By default the errors are expected to be returned by raising an `ApiException` exception.  
This class takes an `IApiError` as parameter.  

So in your application, create a class that implements `IApiError`, then when an error happens throw the `ApiException` that takes as argument an `IApiError` class.  

A practical example:

```c#
public void ThisIsBroken() {
    throw new SampleException(new ApiError(ApiErrorCode.Critial));
}
```

Where `SampleException` is:

```c#
public class SampleException: ApiException<ApiErrorCode> {
    public CDocException(IApiError<ApiErrorCode> error) : base(error) { }
}
```

A sample IApiError class:

```c#
public class ApiError : IApiError<ApiErrorCode> {

    public ApiError(ApiErrorCode errorCode) : this(errorCode, HttpStatusCode.InternalServerError) { }

    public ApiError(ApiErrorCode errorCode, HttpStatusCode httpStatusCode) {
        Code = errorCode;
        Message = ApiErrorMessage.GetMessageForCode(errorCode);
        HttpStatusCode = httpStatusCode;
    }

    public ApiErrorCode Code { get; }

    public string Message { get; }

    public HttpStatusCode HttpStatusCode { get; }
}
```

```c#
public enum ApiErrorCode {
    Critical,
    InvalidValue,
    Unkown
}
```

```c#
internal static class ApiErrorMessage
    public static string GetMessageForCode(ApiErrorCode code) {
        return Messages[code];
    }
}
```

Doing the above will cause a `json` output like:

```json
{
    "code": "Critical",
    "message": "this is bad",
    "httpstatuscode": 500
}
```

## Bad Request Error Code Provider

As part of the startup a `IBadRequestErrorCodeProvider` must be provided, this class will provide the error codes and messages for when input data is incorrect.

A sample implementation:

```c#
public class BadRequestErrorCodeProvider : IBadRequestErrorCodeProvider {
    public string GetCode(string property) {
        if (Enum.TryParse<ApiErrorCode>($"Invalid{property}", out var code)) {
            return code.ToString();
        }
        return nameof(ApiErrorCode.UnknownError);
    }

    public string GetMessageForCode(string code) {
        if (Enum.TryParse<ApiErrorCode>(code, out var apiErrorCode)) {
            return ApiErrorMessage.GetMessageForCode(apiErrorCode);
        }
        return ApiErrorMessage.GetMessageForCode(ApiErrorCode.InvalidValue);
    }
}
```

## Swagger Documentation

To update the swagger documetation so that it is reported what the expected responses are add the following to the class:

```c#
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiError))]
public class WeatherForecastController : ApiController {
    ...
}

This will make it so that every request on that controller shows a `400` and `500` response code and that the respose is of the type `ApiError`.
