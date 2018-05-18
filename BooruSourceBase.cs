using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    public abstract class BooruSourceBase
    {
        public abstract string GetFileUriFromToken(JToken token);

        protected string SourceUri { set; get; }

        public async Task<IEnumerable<string>> Search(params string[] tags)
        {
            //Get request URI
            var requestString = SourceUri + tags.Aggregate((x, y) => $"{x}%20{y}");

            //Setup client
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", Program.client.CurrentUser.Username);

            //Get results stream
            var resultStream = await client.GetStreamAsync(requestString);
            var streamReader = new StreamReader(resultStream);
            if (streamReader.EndOfStream) return new string[0];

            //Return file list
            var resultsObject = await JToken.ReadFromAsync(new JsonTextReader(streamReader));
            return resultsObject.Select(GetFileUriFromToken);
        }
    }
}