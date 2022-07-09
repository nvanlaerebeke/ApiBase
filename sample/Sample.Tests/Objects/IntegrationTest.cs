using System.Text.Json;

namespace Sample.Tests.Objects
{
    public abstract class IntegrationTest
    {
        protected T JsonToObject<T>(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
