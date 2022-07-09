namespace ApiBase.Authentication
{
    /// <summary>
    /// Interface that forces the in and output for an authenticate method
    /// </summary>
    /// <typeparam name="I">Authentication method response object type</typeparam>
    /// <typeparam name="P">Authentication method input parameters object type</typeparam>
    public interface IAuthenticate<I, P>
        where I : IAuthenticationInfo
        where P : IAuthenticationParams
    {
        /// <summary>
        /// Authenticates
        /// </summary>
        /// <param name="authenticationParams"></param>
        /// <returns></returns>
        public abstract I Authenticate(P authenticationParams);
    }
}
