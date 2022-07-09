using AutoMapper;
using Sample.API.Object.API;
using Sample.Process.Object;

namespace ApiBase.Mapping
{
    /// <summary>
    /// Class that contains the translations between the external and internal classes
    /// </summary>
    public class ResponseToDomainProfile : Profile
    {
        /// <summary>
        /// Creates a AutoMapper Profile
        /// </summary>
        public ResponseToDomainProfile()
        {
            _ = CreateMap<Forecast, ForecastResponse>();
            _ = CreateMap<ForecastResponse, Forecast>();
        }
    }
}
