using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BigBeautifulBot
{
    internal class e621Source : BooruSourceBase
    {
        public e621Source()
        {
            SourceUri = "https://e621.net/post/index.json?tags=";
        }

        public override string GetFileUriFromToken(JToken token)
        {
            return (string)token["file_url"];
        }
    }
}