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
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace BigBeautifulBot
{
    public class Program
    {
        public static DiscordSocketClient client;
        public static BBBSettings config;

        public static BigBeautifulBot bbb;
        private static Timer _TickTimer;
        public const string TheChef = "lazorchef#3920";

        public static Random MyRandom { get; } = new Random();

        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args)
        {
            //Load config
            config = new BBBSettings();

            //Initialize BBB
            bbb = new BigBeautifulBot(config);

            _TickTimer = new Timer(Tick, null, config.TickInterval, config.TickInterval);

            //Setup client
            client = new DiscordSocketClient();
            client.Ready += Client_Ready;
            client.JoinedGuild += Client_JoinedGuild;
            client.LeftGuild += Client_LeftGuild;
            client.MessageReceived += Client_MessageReceived;

            //Login and start
            await client.LoginAsync(Discord.TokenType.Bot, config.Token);
            await client.StartAsync();

            //Setup socket client
            var hostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var address = hostInfo.AddressList.First();
            var endpoint = new IPEndPoint(address, 662);
            var socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endpoint);
            socket.Listen(10);

            while(true)
            {
                var connection = socket.Accept();
                var buffer = new byte[1024];
                int receivedBytes;
                string message = string.Empty;
                while((receivedBytes = socket.Receive(buffer)) > 0)
                {
                    message += Encoding.Unicode.GetString(buffer, 0, receivedBytes);
                }
                var socketMessage = new Input.SocketMessage(connection);
                await bbb.MessageReceived(socketMessage);
                connection.Shutdown(SocketShutdown.Both);
                connection.Close();
            }
        }

        private async static Task Client_MessageReceived(SocketMessage arg)
        {
            var discordMessage = new DiscordMessageWrapper(arg);
            await bbb.MessageReceived(discordMessage);
        }

        private static async void Tick(object state)
        {
            try
            {
                if (bbb.Info.Weight > config.MinWeight)
                {
                    bbb.Info.Weight -= config.WeightLossRate;
                }

                if (bbb.Info.Appetite < bbb.Info.MaxAppetite)
                {
                    bbb.Info.Appetite += config.HungerRate;
                }

                await bbb.Info.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        private static async Task Client_LeftGuild(SocketGuild arg)
        {
            await Task.Run(() => Console.WriteLine($"Left: {arg.Name}"));
        }

        private static async Task Client_JoinedGuild(SocketGuild arg)
        {
            await Task.Run(() => Console.WriteLine($"Joined: {arg.Name}"));
        }

        private static async Task Client_Ready()
        {
            Console.WriteLine("Bot Started");
            await client.SetGameAsync(GetRandomElement(bbb.Info.Activities));
        }

        public static string GetRandomElement(string[] array)
        {
            return array[MyRandom.Next(array.Length)];
        }

        public static string GetRandomFile(string dir)
        {
            var files = Directory.GetFiles(dir);
            var file = GetRandomElement(files);
            return file;
        }

        internal static string GenerateStatusBar(decimal fraction)
        {
            bool isNeg = fraction < 0;
            var limit = (isNeg ? 0 : fraction);
            var start = (isNeg ? fraction : 0);

            var result = string.Empty;
            for (var i = start; i < limit; i += 0.1M)
            {
                //TODO: Reverse gradient option
                if (isNeg) result += ":black_heart:";
                else if (i < 1M / 3M) result += ":green_heart:";
                else if (i < 2M / 3M) result += ":yellow_heart:";
                else result += ":heart:";
            }
            return result;
        }
    }
}
