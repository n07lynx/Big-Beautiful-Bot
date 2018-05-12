﻿using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    public class BigBeautifulBot
    {
        private BBBSettings config;
        private BBBInfo bbbInfo;

        public BigBeautifulBot(BBBSettings config)
        {
            this.config = config;
            bbbInfo = new BBBInfo();
        }

        internal async Task MessageReceived(SocketMessage message)
        {
            var messageContent = message.Content;

            //Don't talk to yourself x3
            if (message.Author.Id == Program.client.CurrentUser.Id) return;

            if (messageContent.StartsWith(config.prefix))//Command
            {
                var components = new string(messageContent.Skip(config.prefix.Length).ToArray()).Trim().Split(' ');
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
                }
            }

            if (message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id))//Mention
            {
                await message.Channel.SendMessageAsync("Hi, yes! Did you mention me?");
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
                await message.Channel.SendMessageAsync(":warning: You can't feed more than one person!");
            }
            else if (args.Count() < 1)
            {
                await message.Channel.SendMessageAsync(":warning: Please specify your feedee!");
            }
            else
            {
                var files = Directory.GetFiles(config.progFolder);
                var file = Program.GetRandomElement(files);
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
            if (itemCode == "🍰")//cake?
            {
                bbbInfo.Weight += 0.2M;
                await bbbInfo.Save();
                await message.Channel.SendMessageAsync("Yum! Thanks!");
            }
            else if (itemCode == "⚖")//scales
            {
                await message.Channel.SendMessageAsync($"I currently weigh... ***{bbbInfo.Weight}kgs***!");
            }
            else
            {
                Console.WriteLine($"Unknown ItemCode: {itemCode}");
                await message.Channel.SendMessageAsync("I um... Can't figure out what this is... Beep boop :sweat_smile:");
            }
        }
    }
}