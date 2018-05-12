using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Data.SQLite;

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

            //Initialize BBB
            bbb = new BigBeautifulBot(config);

            db = new SQLiteConnection("Data Source=bbb.db;Version=3;");

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

        private static string GetRandomElement(string[] activities)
        {
            var random = new Random();
            return activities[random.Next(activities.Length)];
        }
    }

    public sealed class BBBSettings : ApplicationSettingsBase
    {
        [UserScopedSetting]
        public string token
        {
            get { return (string)this[nameof(token)]; }
            set { this[nameof(token)] = value; }
        }

        [UserScopedSetting]
        public string prefix
        {
            get { return (string)this[nameof(prefix)]; }
            set { this[nameof(prefix)] = value; }
        }
    }

    public class BigBeautifulBot
    {
        private BBBSettings config;

        public BigBeautifulBot(BBBSettings config)
        {
            this.config = config;
        }

        public BBBInfo bbbInfo { get; private set; }

        internal async Task MessageReceived(SocketMessage message)
        {
            var messageContent = message.Content;

            //Don't talk to yourself x3
            if (message.Author.Id == Program.client.CurrentUser.Id) return;

            if (messageContent.StartsWith(config.prefix))//Command
            {
                var components = messageContent.Skip(config.prefix.Length).ToString().Trim().Split(' ');
                var command = components.First().ToLower();
                var args = components.Skip(1);

                switch (command)
                {
                    case "use":
                        await Use(message, args);
                        break;
                }
            }

            if (message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id))//Mention
            {

            }
        }

        private async Task Use(SocketMessage message, IEnumerable<string> args)
        {
            var itemCode = args.First().ToCharArray().First();
            if (itemCode == 'L')
            {
                //TODO: Move text to resource file
                await message.Channel.SendMessageAsync("I like that letter... I can't really do anything with it though.");
            }
            else if (itemCode == 0x1F370)//cake?
            {
                bbbInfo.Weight += 0.2;
                await bbbInfo.Save();
                await message.Channel.SendMessageAsync("Yum! Thanks!");
            }
            else if (itemCode == 0xFE0F)//scales
            {
                var displayNumber = 1d;
                await message.Channel.SendMessageAsync($"I currently weigh... ***{displayNumber}kgs***!");
            }
            else
            {
                Console.WriteLine($"Unknown ItemCode: {itemCode} ({(int)itemCode})");
                await message.Channel.SendMessageAsync("I um... Can't figure out what this is... Beep boop :sweat_smile:");
            }
        }
    }
}
