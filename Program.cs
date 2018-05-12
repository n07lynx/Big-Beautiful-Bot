using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Data.SQLite;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace BigBeautifulBot
{
    public class Program
    {
        public static DiscordSocketClient client;
        public static BBBSettings config;

        public const decimal kgLbConversionFactor = 0.453592M;
        public static BigBeautifulBot bbb;
        public static SQLiteConnection db;

        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args)
        {
            //Load config
            config = new BBBSettings();

            //Connect to database
            db = new SQLiteConnection("Data Source=bbb.db;Version=3;").OpenAndReturn();

            //Initialize BBB
            bbb = new BigBeautifulBot(config);

            //Setup client
            client = new DiscordSocketClient();
            client.Ready += Client_Ready;
            client.JoinedGuild += Client_JoinedGuild;
            client.LeftGuild += Client_LeftGuild;
            client.MessageReceived += bbb.MessageReceived;

            //Login and start
            await client.LoginAsync(Discord.TokenType.Bot, config.token);
            await client.StartAsync();

            //Hold the program open indefinitely
            await Task.Delay(-1);
        }

        private static async Task Client_LeftGuild(SocketGuild arg)
        {
            Console.WriteLine($"Left: {arg.Name}");
        }

        private static async Task Client_JoinedGuild(SocketGuild arg)
        {
            Console.WriteLine($"Joined: {arg.Name}");
        }

        private static async Task Client_Ready()
        {
            Console.WriteLine("Bot Started");
            await client.SetGameAsync("with her code");
        }

        public static string GetRandomElement(string[] activities)
        {
            var random = new Random();
            return activities[random.Next(activities.Length)];
        }
    }

    public class BBBSettings
    {
        private JObject _jObject;

        public BBBSettings()
        {
            var serialiser = JsonSerializer.Create();
            var configText = File.ReadAllText("config.json");
            _jObject = (JObject)serialiser.Deserialize(new JsonTextReader(new StringReader(configText)));
        }

        public string token => (string)_jObject[nameof(token)];
        public string prefix => (string)_jObject[nameof(prefix)];
        public string progFolder => (string)_jObject[nameof(progFolder)];
    }

    public class BigBeautifulBot
    {
        private BBBSettings config;
        private BBBInfo bbbInfo;

        public BigBeautifulBot(BBBSettings config)
        {
            this.config = config;
            bbbInfo = new BBBInfo();//TODO:Load from database
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
                        break;
                    case "help":
                        await Help(message, args);
                        break;
                    case "feed":
                        await Feed(message, args);
                        break;
                    case "piggy":
                        await Piggy(message, args);
                        break;
                }
            }

            if (message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id))//Mention
            {

            }
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
