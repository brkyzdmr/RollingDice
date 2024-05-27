using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Brkyzdmr.Services.ConfigService
{
    public class JsonDataProvider<T> : IDataProvider<T>
    {
        private readonly JsonSerializerSettings _settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        public void Write(T config, string savePath = null)
        {
            string json = JsonConvert.SerializeObject(config, _settings);
            if (string.IsNullOrEmpty(savePath))
            {
                savePath = Path.Combine(Directory.GetCurrentDirectory(), $"{typeof(T).Name}.json");
            }
            File.WriteAllText(savePath, json);
        }

        public string Serialize(T config)
        {
            return JsonConvert.SerializeObject(config, _settings);
        }

        public T Read(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, _settings);
        }
    }
}