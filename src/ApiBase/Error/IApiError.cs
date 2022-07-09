namespace ApiBase.Error
{
    /// <summary>
    /// The properties in this class will be what is shown in the output class/openapi docs
    /// This uses generics to make the correct documentation (error codes) to show up in the openapi schema's
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApiError<T> : IGeneralApiError
    {
        T Code { get; }
    }
}
