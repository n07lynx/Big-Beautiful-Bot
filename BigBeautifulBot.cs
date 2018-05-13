using System.Linq;
using System.IO;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using BigBeautifulBot.Properties;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BigBeautifulBot
{
    public class BigBeautifulBot
    {
        public BBBSettings Config { get; }
        public BBBInfo Info { get; }
        public FoodProcessor FoodProcessor { get; }
        public Scales Scales { get; }

        public BigBeautifulBot(BBBSettings config)
        {
            Config = config;
            Info = new BBBInfo();
            FoodProcessor = new FoodProcessor(this);
            Scales = new Scales(this);
        }

        internal async Task MessageReceived(SocketMessage message)
        {
            //Don't talk to yourself x3
            if (message.Author.Id == Program.client.CurrentUser.Id) return;

            try
            {
                var messageContent = message.Content;
                if (messageContent.StartsWith(Config.Prefix))//Command
                {
                    var components = new string(messageContent.Skip(Config.Prefix.Length).ToArray()).Trim().Split(' ');
                    var command = components.First().ToLower();
                    var args = components.Skip(1).ToArray();

                    switch (command)
                    {
                        case "use":
                            await Use(message, args);
                            return;
                        case "help":
                            await Help(message, args);
                            return;
                        case "feed":
                            await Feed(message, args);
                            return;
                        case "piggy":
                            await Piggy(message, args);
                            return;
                        case "lbs":
                        case "kgs":
                            await WeightConvert(message, args, command);
                            return;
                        case "lori":
                            await Lori(message, args);
                            return;
                        case "purin":
                            await Purin(message, args);
                            return;
                        case "fatfact":
                            await FatFact(message, args);
                            return;
                        case "fatty":
                            await Fatty(message, args);
                            return;
                    }
                }

                if (message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id))//Mention
                {
                    if (Regex.IsMatch(messageContent, "hi|hello|hey", RegexOptions.IgnoreCase))
                    {
                        await message.Channel.SendMessageAsync(Resources.MentionGreeting);
                    }
                    else if (Regex.IsMatch(messageContent, @"who('?)s a (good|cute) (little )?(fatty|porker|porkchop)\?", RegexOptions.IgnoreCase))
                    {
                        await message.Channel.SendMessageAsync(Resources.MentionWhoIs);
                    }
                    else if(Regex.IsMatch(messageContent, @"(goodnight|'night)", RegexOptions.IgnoreCase))
                    {
                        await message.Channel.SendMessageAsync(string.Format(Resources.MentionGoodnight, message.Author.Mention));
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync(Resources.MentionUnknown);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        private async Task Fatty(SocketMessage message, string[] args)
        {
            var client = new HttpClient();
            var apiUrl = $"http://gelbooru.com/index.php?page=dapi&s=post&q=index&json=1&limit=100&tags=plump";
            var rawJsonStream = await client.GetStreamAsync(apiUrl);
            var booruResults = JToken.ReadFrom(new JsonTextReader(new StreamReader(rawJsonStream)));

            if (booruResults.Any())
            {
                var randomFile = Program.GetRandomElement(booruResults.Select(x => (string)x["file_url"]).ToArray());
                await message.Channel.SendMessageAsync(randomFile);
            }
        }

        private async Task FatFact(SocketMessage message, string[] args)
        {
            var fact = await Info.GetFatFact();
            await message.Channel.SendMessageAsync(fact);
        }

        private async Task Purin(SocketMessage message, string[] args)
        {
            using (message.Channel.EnterTypingState())
            {
                var file = Program.GetRandomFile(Config.PurinFolder);
                await message.Channel.SendFileAsync(file);
            }
        }

        private async Task Lori(SocketMessage message, string[] args)
        {
            using (message.Channel.EnterTypingState())
            {
                var file = Program.GetRandomFile(Config.LorielleFolder);
                await message.Channel.SendFileAsync(file);
            }
        }

        private async Task WeightConvert(SocketMessage message, string[] args, string inputUnits)
        {
            const decimal kgLbConversionFactor = 0.453592M;
            var isKgs = inputUnits == "kgs";
            var isLbs = inputUnits == "lbs";
            var source = int.Parse(args[0]);
            var lbs = isKgs ? source / kgLbConversionFactor : source;
            var kgs = isLbs ? source * kgLbConversionFactor : source;
            var response = $"{source}{inputUnits} is **";
            if (isLbs) response += $"{Math.Round(kgs, 2)}kgs";
            if (isKgs) response += $"{Math.Round(lbs, 2)}lbs";
            response += "**!";
            await message.Channel.SendMessageAsync(response);
        }

        private async Task Piggy(SocketMessage message, string[] args)
        {
            await message.Channel.SendMessageAsync(":pig2:");
        }

        private async Task Feed(SocketMessage message, string[] args)
        {
            if (args.Count() > 1)
            {
                await message.Channel.SendMessageAsync(Resources.FeedErrorTooManyArgs);
            }
            else if (args.Count() < 1)
            {
                await message.Channel.SendMessageAsync(Resources.FeedErrorTooFewArgs);
            }
            else
            {
                string file = Program.GetRandomFile(Config.ProgFolder);
                await message.Channel.SendFileAsync(file, $"{message.Author.Mention} fed {args[0]}");
            }
        }

        private async Task Help(SocketMessage message, string[] args)
        {
            var readmeText = File.ReadAllText("README.md");
            await message.Channel.SendMessageAsync(readmeText);
        }

        private async Task Use(SocketMessage message, string[] args)
        {
            var itemCode = args[0];
            if (itemCode == "⚖")//scales
            {
                await Scales.PerformWeighIn(message);
            }
            else if (itemCode == "<:makuactivate:438142523001667584>" && $"{message.Author.Username}#{message.Author.Discriminator}" == "FairyMaku#0920")
            {
                var adminMessage = Console.ReadLine();
                await message.Channel.SendMessageAsync(adminMessage);
            }
            else if (itemCode == "<:loreille:441422451541278721>")
            {
                await Lori(message, args);
            }
            else if (FoodProcessor.Definitions.ContainsKey(itemCode))
            {
                await FoodProcessor.Consume(itemCode, message);
            }
            else
            {
                Console.WriteLine($"Unknown ItemCode: {itemCode}");
                await message.Channel.SendMessageAsync(Resources.UseUnknown);
            }

            //Save any changes
            await Info.Save();
        }
    }
}