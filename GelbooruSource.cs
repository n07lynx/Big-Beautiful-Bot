using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    internal class GelbooruSource : IBooruSource //TODO: Probably a good idea to use a base class instead to share all the common code
    {
        public GelbooruSource()
        {
            SourceUri = "http://gelbooru.com/index.php?page=dapi&s=post&q=index&json=1&limit=100&tags=";
        }

        public string SourceUri { get; }

        public async Task<IEnumerable<string>> Search(params string[] tags)
        {
            var client = new HttpClient();
            var requestString = SourceUri + tags.Aggregate((x, y) => $"{x}%20{y}");
            var resultStream = await client.GetStreamAsync(requestString);
            var streamReader = new StreamReader(resultStream);
            if (streamReader.EndOfStream) return new string[0];
            var resultsObject = await JToken.ReadFromAsync(new JsonTextReader(streamReader));
            return resultsObject.Select(x => (string)x["file_url"]);
        }
    }
}