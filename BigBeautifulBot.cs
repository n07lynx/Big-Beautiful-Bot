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
        public CommandProcessor CommandProcessor { get; }
        public Scales Scales { get; }

        //Put in info?
        public decimal MaxAppetite => Info.Weight / Config.WeightAppetiteRatio;
        public decimal WellFormedOverfeedLimit => -Math.Abs(Config.OverfeedLimit);
        public bool IsOverfed => Info.Appetite < WellFormedOverfeedLimit;

        Dictionary<string, string> _ResponseMap = new Dictionary<string, string>
        {
            { Resources.RegexGreeting, Resources.MentionGreeting },
            { Resources.RegexWhoIs, Resources.MentionWhoIs},
            { Resources.RegexGoodnight, Resources.MentionGoodnight },
            { Resources.RegexBully, Resources.MentionBully },
            { Resources.RegexLove, Resources.MentionLove }
        };

        public BigBeautifulBot(BBBSettings config)
        {
            Config = config;
            Info = new BBBInfo();
            FoodProcessor = new FoodProcessor(this);
            Scales = new Scales(this);
            CommandProcessor = new CommandProcessor(this);
        }

        internal async Task MessageReceived(SocketMessage message)
        {
            //Don't talk to yourself x3
            if (message.Author.Id == Program.client.CurrentUser.Id) return;

            try
            {
                await CommandProcessor.Process(message);

                if (message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id))//Mention (TODO: Move to a language parser class)
                {
                    var messageContent = message.Content;
                    if (message.Author.ToString() == BBBInfo.TheCreator)//Admin instructions
                    {
                        if (Regex.IsMatch(messageContent, Resources.RegexThatsRight, RegexOptions.IgnoreCase))
                        {
                            await message.Channel.SendMessageAsync(Resources.MentionThatsRight);
                            return;
                        }
                        else if (Regex.IsMatch(messageContent, Resources.RegexFalseAlarm, RegexOptions.IgnoreCase))
                        {
                            await message.Channel.SendMessageAsync(Resources.MentionFalseAlarm);
                            return;
                        }
                    }

                    foreach (var response in _ResponseMap) //Regular mention request/responses
                    {
                        if (Regex.IsMatch(messageContent, response.Key, RegexOptions.IgnoreCase))
                        {
                            await message.Channel.SendMessageAsync(string.Format(response.Value, message.Author.Mention));
                            return;
                        }
                    }

                    //Fallback message
                    await message.Channel.SendMessageAsync(Resources.MentionUnknown);
                }

                if (message.Author.ToString() == Program.TheChef && message.Channel.Name == "oc" && message.Attachments.Any(x => Regex.IsMatch(x.Filename, "discord", RegexOptions.IgnoreCase)))//WAIFU DETECTION SYSTEM
                {
                    await message.Channel.SendMessageAsync($":satellite: W.D.S. (WAIFU DETECTION SYSTEM) ACTIVATED :satellite:\n:incoming_envelope: Notifying The Creator (@{BBBInfo.TheCreator})...");
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
    }
}