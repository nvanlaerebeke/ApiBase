using System;
using System.ComponentModel.DataAnnotations;
using Sample.API.Object.Validation;
using ApiBase.Object;

namespace Sample.API.Object.API
{
    /// <summary>
    /// Details about the Forecast
    /// </summary>
    public class ForecastResponse : IAPIObject
    {
        /// <summary>
        /// Forecast identifier
        /// </summary>
        [Required]
        public string ID { get; set; }

        /// <summary>
        /// Date the forecast is for
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Temperature in degrees Cecilius
        /// </summary>
        [Required]
        public int TemperatureC { get; set; }
        /// <summary>
        /// Temperature in degrees Fahrenheit
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Type of weather
        /// </summary>
        [SummaryValidation]
        [Required]
        public string Summary { get; set; }

        /// <summary>
        /// Returns the object's identifier
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            return ID;
        }
    }
}
