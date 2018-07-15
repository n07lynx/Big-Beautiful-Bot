using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BigBeautifulBot.Input.Inputs;
using Discord.Rest;

namespace BigBeautifulBot.Input
{
    public class SocketMessage : IMessage
    {
        System.Net.Sockets.Socket Connection;

        public SocketMessage(System.Net.Sockets.Socket connection)
        {
            Connection = connection;
            Author = new UserIdentity(connection);
        }

        public UserIdentity Author { get; }
        public string Content { get; internal set; }

        public IDisposable LoadingHandle => new RemoteLoadingHandle(Connection);

        public bool TargetsMe { get; } = true;

        public async Task SendEmbedAsync(string v1, Embed v3)
        {
            foreach(var value in v3)
            {
                await SendMessageAsync($"{value.Key}: {value.Value}");
            }
        }

        public async Task<RestUserMessage> SendFileAsync(string file, string text)
        {
            if (!string.IsNullOrEmpty(text)) await SendMessageAsync(text);
            await Task.Run(() => Connection.SendFile(file));
            throw new NotImplementedException();
        }

        public async Task SendMessageAsync(string response)
        {
            await Task.Run(() => Connection.Send(Encoding.Unicode.GetBytes(response)));
        }
    }
}