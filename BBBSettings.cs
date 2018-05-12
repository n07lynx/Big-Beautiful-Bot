using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace BigBeautifulBot
{
    public class BBBSettings
    {
        private JObject _jObject;

        public BBBSettings()
        {
            var serialiser = JsonSerializer.Create();
            var configText = File.ReadAllText("config.json");
            _jObject = (JObject)serialiser.Deserialize(new JsonTextReader(new StringReader(configText)));
        }

        public string token => (string)_jObject[nameof(token)];
        public string prefix => (string)_jObject[nameof(prefix)];
        public string progFolder => (string)_jObject[nameof(progFolder)];

        public string LorielleFolder => (string)_jObject[nameof(LorielleFolder)];
    }
}