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
using BigBeautifulBot.Input.Processors;
using BigBeautifulBot.Output;

namespace BigBeautifulBot
{
    public class BigBeautifulBot
    {
        public BBBSettings Config { get; }
        public BBBInfo Info { get; }

        //Put in info?

        private List<IInputProcessor> Processors;

        public BigBeautifulBot(BBBSettings config)
        {
            Config = config;
            Info = new BBBInfo(config);
            Processors = new List<IInputProcessor> { new FoodProcessor(this), new CommandProcessor(this), new LanguageProcessor(this) };
        }

        internal async Task MessageReceived(IMessage message)
        {
            //Don't talk to yourself x3
            if (message.Author.SystemName == Program.client.CurrentUser.Mention) return;

            try
            {
                foreach (var processor in Processors)
                {
                    if (processor.TryParse(message, out var input))
                    {
                        await processor.Process(input);
                    }
                }

                // if (message.Author.ToString() == Program.TheChef && message.Channel.Name == "oc" && message.Attachments.Any(x => Regex.IsMatch(x.Filename, "discord", RegexOptions.IgnoreCase)))//WAIFU DETECTION SYSTEM
                // {
                //     await message.Channel.SendMessageAsync($":satellite: W.D.S. (WAIFU DETECTION SYSTEM) ACTIVATED :satellite:\n:incoming_envelope: Notifying The Creator (@{BBBInfo.TheCreator})...");
                //     Console.Beep();
                //     Console.WriteLine("CHECK THE SERVER, LORIELLE COULD HAVE BEEN POSTED!");
                //     Console.Beep();
                // }
            }
            catch (BBBException ex)
            {
                await message.SendMessageAsync(ex.UserMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}