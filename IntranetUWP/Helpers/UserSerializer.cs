using Microsoft.Toolkit.Helpers;
using Newtonsoft.Json;

namespace IntranetUWP.Helpers
{
    public class UserSerializer : IObjectSerializer
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings();
        public UserSerializer()
        {
        }

        public T Deserialize<T>(string value)
        {
            var result = JsonConvert.DeserializeObject<T>(value.ToString(), settings);
            return result;
        }

        public string Serialize<T>(T value)
        {
            var result = JsonConvert.SerializeObject(value, typeof(T), Formatting.Indented, settings);
            return result;
        }
    }
}
