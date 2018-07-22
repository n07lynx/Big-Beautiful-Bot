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
using Discord;
using Discord.Rest;
using BigBeautifulBot.Input;

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
        public static List<ReactionWait> ReactionWaits { get; set; } = new List<ReactionWait>();

        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args)
        {
            var exeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            System.IO.Directory.SetCurrentDirectory(exeDirectory);

            //Load config
            config = new BBBSettings();

            //Initialize BBB
            bbb = new BigBeautifulBot(config);

            _TickTimer = new Timer(Tick, null, config.TickInterval, config.TickInterval);

            //TODO: Move all this client stuff into their own classes

            //Setup client
            client = new DiscordSocketClient();
            client.Ready += Client_Ready;
            client.JoinedGuild += Client_JoinedGuild;
            client.LeftGuild += Client_LeftGuild;
            client.MessageReceived += Client_MessageReceived;
            client.ReactionAdded += Client_ReactionAdded;

            //Login and start
            await client.LoginAsync(Discord.TokenType.Bot, config.Token);
            await client.StartAsync();

            //Setup socket client
            var tcpListener = new TcpListener(IPAddress.Parse("132.148.82.115"), 662);
            tcpListener.Start();
            await ServiceClients(tcpListener);
        }

        private static async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            //TODO: Thread safety!
            var wait = ReactionWaits.SingleOrDefault(x => x.Message == arg1.Id && arg3.UserId == x.ExclusiveInputUser.Id);
            if (wait != null)
            {
                wait.CompletionSource.SetResult(arg3.Emote.Name);
                ReactionWaits.Remove(wait);
            }
        }

        internal static void RegisterReactionWait(TaskCompletionSource<string> completionSource, string[] option, ulong messageId, UserIdentity userId)
        {
            ReactionWaits.Add(new ReactionWait(completionSource, option, messageId, userId));
        }

        private static async Task ServiceClients(TcpListener tcpListener)
        {
            while (true)
            {
                //Each new client, kick off a thread to handle messages
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                new Thread(async () => await HandleClient(tcpClient)).Start();
            }
        }

        private static async Task HandleClient(TcpClient tcpClient)
        {
            try
            {
                using (var connectionStream = tcpClient.GetStream())
                {
                    while (connectionStream.CanRead)
                    {
                        var buff = new byte[1024];
                        var readBytes = connectionStream.Read(buff, 0, buff.Length);
                        if (readBytes > 0)
                        {
                            var socketMessage = new Input.SocketMessage(Encoding.Unicode.GetString(buff, 0, readBytes), connectionStream);
                            await bbb.MessageReceived(socketMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client handling crashed!");
                Console.WriteLine(ex);
            }
        }

        private static Task Client_MessageReceived(Discord.WebSocket.SocketMessage arg)
        {
            try
            {
                var discordMessage = new DiscordMessageWrapper(arg);
                _ = bbb.MessageReceived(discordMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //HACK: See Discord.Net/issues/1115
            return Task.CompletedTask;
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
