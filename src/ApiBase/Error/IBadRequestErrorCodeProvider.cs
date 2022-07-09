namespace ApiBase.Error
{
    public interface IBadRequestErrorCodeProvider
    {
        string GetCode(string property);

        string GetMessageForCode(string property);
    }
}
