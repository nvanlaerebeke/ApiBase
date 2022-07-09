using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Process.Object;
using ApiBase.Filter.Pagination;

namespace Sample.Process
{
    public class WeatherForecasts : IWeatherForecasts
    {
        private static List<Forecast> Forecasts;

        public WeatherForecasts()
        {
            if (Forecasts == null)
            {
                var task = GenerateForecasts();
                task.Wait();
                Forecasts = task.Result;
            }
        }

        public async Task<PagedResult<Forecast>> GetAsync(PaginationFilter pageFilter = null, ApiBase.Filter.Sorting.SortFilter sortFilter = null)
        {
            return await Task.Run(() =>
            {
                List<Forecast> result = new List<Forecast>(Forecasts);
                if (sortFilter != null && !string.IsNullOrEmpty(sortFilter.OrderBy))
                {
                    switch (sortFilter.OrderBy.ToLower())
                    {
                        case "date":
                            result = result.OrderBy(x => x.Date).ToList();
                            break;

                        case "id":
                            result = result.OrderBy(x => x.ID).ToList();
                            break;

                        case "summary":
                            result = result.OrderBy(x => x.Summary).ToList();
                            break;

                        case "temperaturec":
                            result = result.OrderBy(x => x.TemperatureC).ToList();
                            break;
                    }
                }
                if (pageFilter?.PageSize > 0)
                {
                    result = Forecasts.Skip(pageFilter.PageNumber.Value * pageFilter.PageSize.Value).Take(pageFilter.PageSize.Value).ToList();
                }

                return new PagedResult<Forecast>(result, Forecasts.Count, pageFilter);
            }).ConfigureAwait(false);
        }

        public Forecast Add(Forecast foreCast)
        {
            foreCast.ID = Guid.NewGuid().ToString();
            Forecasts.Add(foreCast);
            return foreCast;
        }

        public Forecast Get(string id)
        {
            return Forecasts.Find(f => f.ID.Equals(id));
        }

        public Forecast Delete(string id)
        {
            var toRemove = Forecasts.Find(f => f.ID.Equals(id));
            if (toRemove == null)
            {
                return null;
            }
            _ = Forecasts.RemoveAll(x => x.ID.Equals(id));
            return toRemove;
        }

        public Forecast Update(Forecast updated)
        {
            var old = Get(updated.ID);
            if (old == null)
            {
                return null;
            }
            old.Date = updated.Date;
            old.Summary = updated.Summary;
            old.TemperatureC = updated.TemperatureC;

            return old;
        }

        /// <summary>
        /// Returns some random forecasts
        /// </summary>
        /// <returns></returns>
        private static async Task<List<Forecast>> GenerateForecasts()
        {
            return await Task.Run(() =>
            {
                var rng = new Random();
                var summaries = Enum.GetValues<Summary>().ToList();
                return new List<Forecast>(Enumerable.Range(1, 98).Select(index => new Forecast
                {
                    ID = Guid.NewGuid().ToString(),
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = summaries[rng.Next(summaries.Count)].ToString()
                }))
                {
                    GetTestForcast(),
                    GetProtectedTestForcast()
                };
            }).ConfigureAwait(false);
        }

        private static Forecast GetTestForcast()
        {
            return new Forecast()
            {
                ID = "b2216615-de2e-457b-99c1-0376fddecc04",
                Date = DateTime.Now,
                Summary = nameof(Summary.Mild),
                TemperatureC = 20
            };
        }

        private static Forecast GetProtectedTestForcast()
        {
            return new Forecast()
            {
                ID = "972944b0-38c3-4b43-a56d-b773ebf11a11",
                Date = DateTime.Now,
                Summary = nameof(Summary.Mild),
                TemperatureC = 20
            };
        }

        public static void Reset()
        {
            Forecasts = null;
        }
    }
}
