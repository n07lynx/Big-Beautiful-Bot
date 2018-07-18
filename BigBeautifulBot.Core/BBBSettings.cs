using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace BigBeautifulBot
{
    public class BBBSettings
    {
        private JToken _jObject;

        public BBBSettings()
        {
            _jObject = JToken.ReadFrom(new JsonTextReader(new StreamReader("config.json")));
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
        public int TickInterval => (int)_jObject[nameof(TickInterval)];
        public string GeneralSizesFolder => (string)_jObject[nameof(GeneralSizesFolder)];
        public string DefaultImageSource => (string)_jObject[nameof(DefaultImageSource)];
    }
}