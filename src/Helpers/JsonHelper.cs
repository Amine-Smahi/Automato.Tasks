using System.Text.Json;

namespace Automato.Tasks.Helpers
{
    public static class JsonHelper
    {
        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}