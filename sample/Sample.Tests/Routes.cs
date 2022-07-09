namespace Sample.Tests
{
    internal static class Routes
    {
        public const string Base = "";

        public static class WeatherForecast
        {
            internal static string GetAllWithPathBase() => Base + "/sample/WeatherForecast";

            internal static string GetAll() => Base + "/WeatherForecast";

            internal static string Create() => GetAll();

            internal static string Get(string id) => Base + "/WeatherForecast/" + id;

            internal static string Delete(string id) => Get(id);

            internal static string Update() => Create();
        }
    }
}
