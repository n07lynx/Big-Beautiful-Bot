﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace BigBeautifulBot
{
    internal class SafebooruSource : BooruSourceBase
    {
        public SafebooruSource()
        {
            SourceUri = "http://safebooru.org/index.php?page=dapi&s=post&q=index&json=1&limit=100&tags=";
        }

        public override string GetFileUriFromToken(JToken token)
        {
            return Path.Combine("http://safebooru.org/", (string)token["file_url"]);
        }
    }
}