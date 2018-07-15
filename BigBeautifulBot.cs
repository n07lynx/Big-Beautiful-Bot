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

        //Put in info?
        public decimal MaxAppetite => Info.Weight / Config.WeightAppetiteRatio;
        public decimal WellFormedOverfeedLimit => -Math.Abs(Config.OverfeedLimit);
        public bool IsOverfed => Info.Appetite < WellFormedOverfeedLimit;

        private List<RequestProcessorBase> Processors;

        public BigBeautifulBot(BBBSettings config)
        {
            Config = config;
            Info = new BBBInfo();
            Processors = new List<RequestProcessorBase> { new FoodProcessor(this), new CommandProcessor(this) };
        }

        internal async Task MessageReceived(SocketMessage message)
        {
            //Don't talk to yourself x3
            if (message.Author.Id == Program.client.CurrentUser.Id) return;

            try
            {
                foreach (var processor in Processors)
                {
                    await processor.Process(message);
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