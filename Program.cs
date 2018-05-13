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

            _TickTimer = new Timer(Tick, null, 60000, 60000);

            //Setup client
            client = new DiscordSocketClient();
            client.Ready += Client_Ready;
            client.JoinedGuild += Client_JoinedGuild;
            client.LeftGuild += Client_LeftGuild;
            client.MessageReceived += bbb.MessageReceived;

            //Login and start
            await client.LoginAsync(Discord.TokenType.Bot, config.Token);
            await client.StartAsync();

            //Hold the program open indefinitely
            await Task.Delay(-1);
        }

        private static async void Tick(object state)
        {
            try
            {
                if (bbb.Info.Weight > config.MinWeight)
                {
                    bbb.Info.Weight -= config.WeightLossRate;
                    await bbb.Info.Save();
                }

                if(bbb.Info.Appetite < bbb.Info.Weight / config.WeightAppetiteRatio)
                {
                    bbb.Info.Appetite += config.HungerRate;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
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
            await client.SetGameAsync(GetRandomElement(bbb.Info.Activities));
        }

        public static string GetRandomElement(string[] array)
        {
            var random = new Random();
            return array[random.Next(array.Length)];
        }

        public static string GetRandomFile(string dir)
        {
            var files = Directory.GetFiles(dir);
            var file = GetRandomElement(files);
            return file;
        }
    }
}
