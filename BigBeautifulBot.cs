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
using System.Collections.Generic;

namespace BigBeautifulBot
{
    public class BigBeautifulBot
    {
        public BBBSettings Config { get; }
        public BBBInfo Info { get; }
        public FoodProcessor FoodProcessor { get; }
        public Scales Scales { get; }

        //Put in info?
        public decimal MaxAppetite => Info.Weight / Config.WeightAppetiteRatio;
        public decimal WellFormedOverfeedLimit => -Math.Abs(Config.OverfeedLimit);
        public bool IsOverfed => Info.Appetite < WellFormedOverfeedLimit;

        //Booru stuff
        private const string sourceRegex = @"^\(\S+\)$";
        public Dictionary<string, BooruSourceBase> BooruSources = new Dictionary<string, BooruSourceBase>(StringComparer.CurrentCultureIgnoreCase) { { "gelbooru", new GelbooruSource() }, { "safebooru", new SafebooruSource() }, { "e621", new e621Source() } };
        public string[] CommonTags = { "fat", "female", "-1boy", "-fat_man", "-shota", "-loli" };

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
                        case "fatornot":
                            await FatOrNot(message, args);
                            return;
                        case "status":
                            await Status(message, args);
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
                    else if (Regex.IsMatch(messageContent, @"goodnight|'night|お(やす|休)み", RegexOptions.IgnoreCase))
                    {
                        await message.Channel.SendMessageAsync(string.Format(Resources.MentionGoodnight, message.Author.Mention));
                    }
                    else if (Regex.IsMatch(messageContent, @"you('?)re( a)?[^\.]*(fat|pork|glutton|chubby|pig)", RegexOptions.IgnoreCase))
                    {
                        await message.Channel.SendMessageAsync(Resources.MentionBully);
                    }
                    else if (Regex.IsMatch(messageContent, @"isn('?)t that right\?", RegexOptions.IgnoreCase) && message.Author.ToString() == Program.TheCreator)
                    {
                        await message.Channel.SendMessageAsync(Resources.MentionThatsRight);
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync(Resources.MentionUnknown);
                    }
                }

                if (message.Author.ToString() == "lazorchef#3920" && message.Channel.Name == "oc" && message.Attachments.Any())//WAIFU DETECTION SYSTEM
                {
                    await message.Channel.SendMessageAsync($":satellite: W.D.S. (WAIFU DETECTION SYSTEM) ACTIVATED :satellite:\n:incoming_envelope: Notifying The Creator (@{Program.TheCreator})...");
                    Console.Beep();
                    Console.WriteLine("CHECK THE SERVER, LORIELLE COULD HAVE BEEN POSTED!");
                    Console.Beep();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        private async Task Status(SocketMessage message, string[] args)
        {
            var builder = new Discord.EmbedBuilder();
            builder.AddInlineField(nameof(Info.Weight), $"{Info.Weight}kgs");
            builder.AddInlineField(nameof(Info.Appetite), IsOverfed ? ":heartpulse: __OVERFED__ :heartpulse:" : Program.GenerateStatusBar(Info.Appetite / MaxAppetite));
            await message.Channel.SendMessageAsync("**Current Status**", false, builder.Build());
        }

        private async Task FatOrNot(SocketMessage message, string[] args)
        {
            const string tick = "✅";
            var folders = Directory.GetDirectories(Config.GeneralSizesFolder);
            var foldersBySize = folders.ToDictionary(x => int.Parse(x.Split(' ').Last()), x => x);
            var checkFolderNumber = Program.MyRandom.Next(foldersBySize.Keys.Min(), foldersBySize.Keys.Max());
            var checkfolder = foldersBySize[checkFolderNumber];

            //Get two unique images
            var image1 = Program.GetRandomFile(checkfolder);
            string image2;
            do image2 = Program.GetRandomFile(checkfolder);//TODO: Prevent infinite loop
            while (image1 == image2);

            await message.Channel.SendMessageAsync($"Who's fatter?");

            //Post the images and make the options obvious
            var message1 = await message.Channel.SendFileAsync(image1);
            var message2 = await message.Channel.SendFileAsync(image2);
            await message1.AddReactionAsync(new Discord.Emoji(tick));
            await message2.AddReactionAsync(new Discord.Emoji(tick));

            //Wait 15 seconds for users to vote
            await Task.Delay(15000);

            //Collect vote results
            var r1 = await message1.GetReactionUsersAsync(tick);
            var r2 = await message2.GetReactionUsersAsync(tick);

            if (r1.Count != r2.Count)
            {
                //Calculate the position to move the image to
                var relativeFatness = r1.Count > r2.Count ? 1 : -1;
                var target1 = checkFolderNumber + relativeFatness;
                var target2 = checkFolderNumber - relativeFatness;

                if (foldersBySize.ContainsKey(target1))
                {
                    //Move image 1 up/down
                    var fileName = Path.GetFileName(image1);
                    var newFolderLocation = Path.Combine(foldersBySize[target1], fileName);
                    File.Move(image1, newFolderLocation);
                }
                else if (foldersBySize.ContainsKey(target2))
                {
                    //Move image 2 up/down
                    var fileName = Path.GetFileName(image2);
                    var newFolderLocation = Path.Combine(foldersBySize[target2], fileName);
                    File.Move(image2, newFolderLocation);
                }
                else
                {
                    throw new Exception("This shouldn't happen but I'm pretty stupid so I wouldn't put it past me.");
                }
            }

            await message.Channel.SendMessageAsync("Thanks for voting! Maku's cutefats folder has been updated.");
        }

        private async Task Fatty(SocketMessage message, string[] args)
        {
            //Build tag list
            var inTags = args.Where(x => !Regex.IsMatch(x, sourceRegex));
            var tags = CommonTags.Concat(inTags);

            //Determine sources
            var inSources = args.Where(x => Regex.IsMatch(x, sourceRegex)).ToList();
            if (!inSources.Any()) inSources.Add(Config.DefaultImageSource);

            List<string> files = new List<string>();
            foreach (var source in inSources)
            {
                var sourceKey = source.Trim('(', ')');
                BooruSourceBase booruSource;
                if (BooruSources.TryGetValue(sourceKey, out booruSource))
                {
                    files.AddRange(await booruSource.Search(tags.ToArray()));
                }
                else
                {
                    await message.Channel.SendMessageAsync(string.Format(Resources.FattyErrorUnknownSource, sourceKey));
                    return;
                }
            }

            if (files.Any())
            {
                //Send result to client
                var randomFile = Program.GetRandomElement(files.ToArray());
                await message.Channel.SendMessageAsync(randomFile);
            }
            else
            {
                //No results
                await message.Channel.SendMessageAsync(Resources.FattyErrorNoResults);
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
            if (!args.Any())
            {
                await message.Channel.SendMessageAsync(string.Format(Resources.WeightConvertErrorTooFewArgs, inputUnits));
                return;
            }

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
            await message.Channel.SendMessageAsync(Program.GetRandomElement(new string[] { ":pig2:", ":pig:", ":pig_nose:" }));
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
            if (!args.Any())
            {
                await message.Channel.SendMessageAsync(Resources.UseErrorTooFewArgs);
                return;
            }

            var itemCode = args[0];
            if (itemCode == "⚖")//scales
            {
                await Scales.PerformWeighIn(message);
            }
            else if (itemCode == "<:makuactivate:438142523001667584>" && message.Author.ToString() == Program.TheCreator)
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