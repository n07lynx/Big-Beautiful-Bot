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
    internal class GelbooruSource : BooruSourceBase
    {
        public GelbooruSource()
        {
            SourceUri = "http://gelbooru.com/index.php?page=dapi&s=post&q=index&json=1&limit=100&tags=";
        }

        public override string GetFileUriFromToken(JToken token)
        {
            return (string)token["file_url"];
        }
    }
}