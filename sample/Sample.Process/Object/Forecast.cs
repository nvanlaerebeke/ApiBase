using System;

namespace Sample.Process.Object
{
    public class Forecast
    {
        public string ID { get; set; }
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
