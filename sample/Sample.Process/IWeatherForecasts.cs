using System.Threading.Tasks;
using Sample.Process.Object;
using ApiBase.Filter.Pagination;
using ApiBase.Filter.Sorting;

namespace Sample.Process
{
    public interface IWeatherForecasts
    {
        Forecast Add(Forecast foreCast);
        Forecast Delete(string id);
        Forecast Get(string id);
        Task<PagedResult<Forecast>> GetAsync(PaginationFilter pageFilter = null, SortFilter sortFilter = null);
        Forecast Update(Forecast updated);
    }
}