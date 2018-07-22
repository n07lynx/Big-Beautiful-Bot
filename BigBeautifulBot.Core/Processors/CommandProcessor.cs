using BigBeautifulBot.Input;
using BigBeautifulBot.Output;
using BigBeautifulBot.Properties;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    public class CommandProcessor : InputProcessorBase<CommandInput>
    {
        public CommandProcessor(BigBeautifulBot bot) : base(bot)
        {
            Scales = new Scales(bot);
        }

        public override async Task Process(CommandInput message)
        {
            switch (message.CommandName)
            {
                case "use":
                    await Use(message);
                    return;
                case "help":
                    await Help(message);
                    return;
                case "feed":
                    await Feed(message);
                    return;
                case "piggy":
                    await Piggy(message);
                    return;
                case "lbs":
                case "kgs":
                    await WeightConvert(message);
                    return;
                case "lori":
                    await Lori(message);
                    return;
                case "purin":
                    await Purin(message);
                    return;
                case "fatfact":
                    await FatFact(message);
                    return;
                case "fatty":
                    await Fatty(message);
                    return;
                case "fatornot":
                    await FatOrNot(message);
                    return;
                case "status":
                    await Status(message);
                    return;
                case "name":
                    await Name(message);
                    return;
                case "savefat":
                    await SaveFat(message);
                    return;
            }
        }

        private const string TwoDimensionOptionEmoji = "2⃣";
        private const string ThreeDimensionOptionEmoji = "3⃣";
        private const string WeightGainOptionEmoji = "⬆";

        private async Task SaveFat(CommandInput message)
        {
            if (!message.Args.Any())
            {
                throw new BBBException(Resources.SaveFatErrorTooFewArgs);
            }

            if (!message.Author.IsAdmin)
            {
                throw new BBBException(Resources.SaveFatErrorAccessDenied);
            }

            if (Uri.TryCreate(message.Args.First(), UriKind.Absolute, out var path))
            {
                using (var client = new System.Net.Http.HttpClient())
                using (var response = await client.GetAsync(path))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new BBBException(Resources.SaveFatErrorInternetResourceUnavailable);
                    }

                    var fileBytes = await response.Content.ReadAsByteArrayAsync();

                    var @out = await message.Respond($"Where does this fatty belong?\n({TwoDimensionOptionEmoji})D, ({ThreeDimensionOptionEmoji})D or Weight ({WeightGainOptionEmoji}) Gain.");
                    var selection = await @out.PromptOptions(message.Author, TwoDimensionOptionEmoji, ThreeDimensionOptionEmoji, WeightGainOptionEmoji);

                    string folder;
                    switch (selection)
                    {
                        case TwoDimensionOptionEmoji:
                            folder = Path.Combine(_Bot.Config.GeneralSizesFolder, "Size Class 5");
                            break;
                        case ThreeDimensionOptionEmoji:
                            folder = _Bot.Config.ThreeDimensionalFatsFolder;
                            break;
                        case WeightGainOptionEmoji:
                            folder = _Bot.Config.ProgFolder;
                            break;
                        default:
                            throw new Exception();
                    }

                    string fileName = Path.GetFileName(path.LocalPath);
                    var fullPath = Path.Combine(folder, fileName);
                    await File.WriteAllBytesAsync(fullPath, fileBytes);

                    await message.Respond("Fatty saved successfully!");

                }
            }
        }

        private async Task Name(CommandInput message)
        {
            if (message.Author.ToString().Equals(BBBInfo.TheCreator))
            {
                await Program.client.CurrentUser.ModifyAsync(x => x.Username = message.Args.Aggregate((y, z) => $"{y} {z}"));
            }
            else
            {
                await message.Respond(Resources.ErrorAccessDenied);
            }
        }

        private async Task FatOrNot(CommandInput command)
        {
            const string tick = "✅";
            var folders = Directory.GetDirectories(_Bot.Config.GeneralSizesFolder);
            var foldersBySize = folders.ToDictionary(x => int.Parse(x.Split(' ').Last()), x => x);
            var checkFolderNumber = Program.MyRandom.Next(foldersBySize.Keys.Min(), foldersBySize.Keys.Max());
            var checkfolder = foldersBySize[checkFolderNumber];

            //Get two unique images
            var image1 = Program.GetRandomFile(checkfolder);
            string image2;
            do image2 = Program.GetRandomFile(checkfolder);//TODO: Prevent infinite loop
            while (image1 == image2);

            await command.Respond($"Who's fatter?");

            //Post the images and make the options obvious
            var message1 = await command.FileRespond(image1);
            await message1.AddScalarUI(tick);
            var message2 = await command.FileRespond(image2);
            await message2.AddScalarUI(tick);

            //Wait 15 seconds for users to vote
            await Task.Delay(15000);

            //Collect vote results
            var r1 = await message1.GetScalarUI(tick);
            var r2 = await message2.GetScalarUI(tick);

            if (r1 != r2)
            {
                //Calculate the position to move the image to
                var relativeFatness = r1 > r2 ? 1 : -1;
                var target1 = checkFolderNumber + relativeFatness;
                var target2 = checkFolderNumber - relativeFatness;

                if (foldersBySize.ContainsKey(target1))
                {
                    //Move image 1 up/down
                    var fileName = Path.GetFileName(image1);
                    var newFolderLocation = Path.Combine(foldersBySize[target1], fileName);
                    if(File.Exists(newFolderLocation)) return; //TODO: Handle this
                    File.Move(image1, newFolderLocation);
                }
                else if (foldersBySize.ContainsKey(target2))
                {
                    //Move image 2 up/down
                    var fileName = Path.GetFileName(image2);
                    var newFolderLocation = Path.Combine(foldersBySize[target2], fileName);
                    if(File.Exists(newFolderLocation)) return; //TODO: Handle this
                    File.Move(image2, newFolderLocation);
                }
                else
                {
                    throw new Exception("This shouldn't happen but I'm pretty stupid so I wouldn't put it past me.");
                }
            }

            await command.Respond("Thanks for voting! Maku's cutefats folder has been updated.");
        }

        private async Task WeightConvert(CommandInput command)
        {
            if (!command.Args.Any())
            {
                throw new BBBException(string.Format(Resources.WeightConvertErrorTooFewArgs, command.CommandName));
            }

            const decimal kgLbConversionFactor = 0.453592M;
            var isKgs = command.CommandName == "kgs";
            var isLbs = command.CommandName == "lbs";
            var source = int.Parse(command.Args[0]);
            var lbs = isKgs ? source / kgLbConversionFactor : source;
            var kgs = isLbs ? source * kgLbConversionFactor : source;
            var response = $"{source}{command.CommandName} is **";
            if (isLbs) response += $"{Math.Round(kgs, 2)}kgs";
            if (isKgs) response += $"{Math.Round(lbs, 2)}lbs";
            response += "**!";
            await command.Respond(response);
        }

        private async Task Status(CommandInput message)
        {
            var embed = new Embed();
            embed.Add(nameof(_Bot.Info.Weight), $"{_Bot.Info.Weight}kgs");
            embed.Add(nameof(_Bot.Info.Appetite), _Bot.Info.IsOverfed ? ":heartpulse: __OVERFED__ :heartpulse:" : Program.GenerateStatusBar(_Bot.Info.Appetite / _Bot.Info.MaxAppetite));
            await message.Message.SendEmbedAsync("**Current Status**", embed);
        }


        //Booru stuff
        private const string sourceRegex = @"^\(\S+\)$";
        public Dictionary<string, BooruSourceBase> BooruSources = new Dictionary<string, BooruSourceBase>(StringComparer.CurrentCultureIgnoreCase) { { "gelbooru", new GelbooruSource() }, { "safebooru", new SafebooruSource() }, { "e621", new e621Source() } };

        public Scales Scales { get; }

        public string[] CommonTags = { "fat", "female", "-1boy", "-fat_man", "-shota", "-loli" };

        private async Task Fatty(CommandInput command)
        {
            //Build tag list
            var inTags = command.Args.Where(x => !Regex.IsMatch(x, sourceRegex));
            var tags = CommonTags.Concat(inTags);

            //Determine sources
            var inSources = command.Args.Where(x => Regex.IsMatch(x, sourceRegex)).ToList();
            if (!inSources.Any()) inSources.Add(_Bot.Config.DefaultImageSource);

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
                    throw new BBBException(string.Format(Resources.FattyErrorUnknownSource, sourceKey));
                }
            }

            if (files.Any())
            {
                //Send result to client
                var randomFile = Program.GetRandomElement(files.ToArray());
                await command.Respond(randomFile);
            }
            else
            {
                //No results
                await command.Respond(Resources.FattyErrorNoResults);
            }
        }

        private async Task FatFact(CommandInput command)
        {
            var fact = await _Bot.Info.GetFatFact();
            await command.Respond(fact);
        }

        private async Task Purin(CommandInput command)
        {
            using (command.LoadingHandle)
            {
                var file = Program.GetRandomFile(_Bot.Config.PurinFolder);
                await command.Respond(file);
            }
        }

        private async Task Lori(CommandInput command)
        {
            using (command.LoadingHandle)
            {
                var file = Program.GetRandomFile(_Bot.Config.LorielleFolder);
                await command.FileRespond(file);
            }
        }

        private async Task Piggy(CommandInput command)
        {
            await command.Respond(Program.GetRandomElement(new string[] { ":pig2:", ":pig:", ":pig_nose:" }));
        }

        private async Task Feed(CommandInput command)
        {
            if (command.Args.Count() > 1)
            {
                await command.Respond(Resources.FeedErrorTooManyArgs);
            }
            else if (!command.Args.Any())
            {
                await command.Respond(Resources.FeedErrorTooFewArgs);
            }
            else
            {
                string file = Program.GetRandomFile(_Bot.Config.ProgFolder);
                await command.FileRespond(file, $"{command.Author.SystemName} fed {command.Args[0]}");
            }
        }

        private async Task Help(CommandInput message)
        {
            var readmeText = File.ReadAllText("README.md");
            await message.Respond(readmeText);
        }

        public override bool TryParse(IMessage message, out IInput command)
        {
            var messageContent = message.Content;
            if (messageContent.StartsWith(_Bot.Config.Prefix))
            {
                var components = new string(messageContent.Skip(_Bot.Config.Prefix.Length).ToArray()).Trim().Split(' ');
                var commandName = components.First().ToLower();
                var args = components.Skip(1).ToArray();
                command = new CommandInput(message) { CommandName = commandName, Args = args };
                return true;
            }
            else
            {
                command = null;
                return false;
            }
        }

        private async Task Use(CommandInput command)
        {
            if (!command.Args.Any())
            {
                throw new BBBException(Resources.UseErrorTooFewArgs);
            }

            var itemCode = command.Args[0];
            if (itemCode.Equals("⚖", StringComparison.InvariantCultureIgnoreCase))//scales 
            {
                await Scales.PerformWeighIn(command);
            }
            else if (itemCode == "<:makuactivate:438142523001667584>" && command.Author.IsAdmin)
            {
                var adminMessage = Console.ReadLine();
                await command.Respond(adminMessage);
            }
            else if (itemCode == "<:loreille:441422451541278721>")
            {
                await Lori(command);
            }
            else
            {
                Console.WriteLine($"Unknown ItemCode: {itemCode}");
                await command.Respond(Resources.UseUnknown);
            }

            //Save any changes 
            await _Bot.Info.Save();
        }
    }
}
