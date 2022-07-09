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
    
}
```

A sample IApiError class:

```c#
public class ApiError : IApiError<ApiErrorCode>

    public ApiError(ApiErrorCode errorCode) : this(errorCode, HttpStatusCode.InternalServerError) { }

    public ApiError(ApiErrorCode errorCode, HttpStatusCode httpStatusCode) {
        Code = errorCode.ToString();
        Message = ApiErrorMessage.GetMessageForCode(errorCode);
        HttpStatusCode = httpStatusCode;
    }

    string Code { get; }

    string Message { get; }

    HttpStatusCode HttpStatusCode { get; }
}
```

```c#
public enum ApiErrorCode {
    Critical
}
```

```c#
public static class ApiErrorMessage {
    private static readonly Dictionary<ApiErrorCode, string> Messages = new() {
        { ApiErrorCode.Critical, "this is bad" }
    };

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
