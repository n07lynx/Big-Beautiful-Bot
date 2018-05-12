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
using System.Threading;

namespace BigBeautifulBot
{
    public class Program
    {
        public static DiscordSocketClient client;
        public static BBBSettings config;

        public static BigBeautifulBot bbb;
        private static Timer _TickTimer;

        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args)
        {
            //Load config
            config = new BBBSettings();

            //Initialize BBB
            bbb = new BigBeautifulBot(config);

            _TickTimer = new Timer(Tick, null, 0, 60000);

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

        private static async void Tick(object state)
        {
            if (bbb.Info.Weight > 55)
            {
                bbb.Info.Weight -= 0.01M;
                await bbb.Info.Save();
            }
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
}
