using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

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
    }
}