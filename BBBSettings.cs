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

        public string Token => (string)_jObject[nameof(Token)];
        public string Prefix => (string)_jObject[nameof(Prefix)];
        public string ProgFolder => (string)_jObject[nameof(ProgFolder)];
        public string LorielleFolder => (string)_jObject[nameof(LorielleFolder)];
        public string PurinFolder => (string)_jObject[nameof(PurinFolder)];
        public decimal WeightLossRate => (decimal)_jObject[nameof(WeightLossRate)];
        public decimal MinWeight => (decimal)_jObject[nameof(MinWeight)];
        public decimal HungerRate => (decimal)_jObject[nameof(HungerRate)];
        public decimal WeightAppetiteRatio => (decimal)_jObject[nameof(WeightAppetiteRatio)];
        public decimal OverfeedLimit => (decimal)_jObject[nameof(OverfeedLimit)];
    }
}