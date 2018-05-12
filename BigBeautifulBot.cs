using System.Linq;
using System.IO;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using BigBeautifulBot.Properties;

namespace BigBeautifulBot
{
    public class BigBeautifulBot
    {
        private BBBSettings _Config;
        public BBBInfo Info { get; }
        public FoodProcessor FoodProcessor { get; }

        public BigBeautifulBot(BBBSettings config)
        {
            _Config = config;
            Info = new BBBInfo();
            FoodProcessor = new FoodProcessor();
        }

        internal async Task MessageReceived(SocketMessage message)
        {
            //Don't talk to yourself x3
            if (message.Author.Id == Program.client.CurrentUser.Id) return;

            try
            {
                var messageContent = message.Content;
                if (messageContent.StartsWith(_Config.prefix))//Command
                {
                    var components = new string(messageContent.Skip(_Config.prefix.Length).ToArray()).Trim().Split(' ');
                    var command = components.First().ToLower();
                    var args = components.Skip(1).ToArray();

                    using (message.Channel.EnterTypingState())
                    {
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
                        }
                    }
                }

                if (message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id))//Mention
                {
                    await message.Channel.SendMessageAsync(Resources.MentionUnknown);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        private async Task Lori(SocketMessage message, string[] args)
        {
            var file = Program.GetRandomFile(_Config.LorielleFolder);
            await message.Channel.SendFileAsync(file);
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
                string file = Program.GetRandomFile(_Config.progFolder);
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
            if (itemCode == "🍰")//cake
            {
                Info.Weight += 0.2M;
                await message.Channel.SendMessageAsync(Resources.UseShortcake);
            }
            else if (itemCode == "⚖")//scales
            {
                await message.Channel.SendMessageAsync(string.Format(Resources.UseScales, Info.Weight));
            }
            else if (itemCode == "<:makuactivate:438142523001667584>" && $"{message.Author.Username}#{message.Author.Discriminator}" == "FairyMaku#0920")
            {
                var adminMessage = Console.ReadLine();
                await message.Channel.SendMessageAsync(adminMessage);
            }
            else if (itemCode == "<:cupcake:409416270534934529>")
            {
                Info.Weight += 0.15M;
                await message.Channel.SendMessageAsync(Resources.UseCupcake);
            }
            else if (itemCode == "🥞")
            {
                Info.Weight += 0.19M;
                await message.Channel.SendMessageAsync(Resources.UsePancake);
            }
            else if (itemCode == "<:loreille:441422451541278721>")
            {
                await Lori(message, args);
            }
            else if (itemCode == "🍮")
            {
                Info.Weight += 0.22M;
                await message.Channel.SendMessageAsync(Resources.UseCustard);
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