using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BigBeautifulBot.Input;
using BigBeautifulBot.Output;
using Discord.Rest;

namespace BigBeautifulBot.Input
{
    public class SocketMessage : IMessage
    {
        public SocketMessage(string v, NetworkStream connectionStream)
        {
            ReplyChannel = connectionStream;
            Content = v;
            Author = new UserIdentity("Unknown Socket User");
        }

        public UserIdentity Author { get; }
        public string Content {get;}
        public IDisposable LoadingHandle => new RemoteLoadingHandle();
        public bool TargetsMe { get; } = true;
        public NetworkStream ReplyChannel { get; private set; }

        public async Task SendEmbedAsync(string v1, Embed v3)
        {
            foreach(var value in v3)
            {
                await SendMessageAsync($"{value.Key}: {value.Value}");
            }
        }

        public async Task<OutputBase> SendFileAsync(string file, string text)
        {
            if (!string.IsNullOrEmpty(text)) await SendMessageAsync(text);
            await Task.Run(() => ReplyChannel.Write(Encoding.Unicode.GetBytes(file)));
            throw new NotImplementedException();
        }

        public async Task<OutputBase> SendMessageAsync(string response)
        {
            await Task.Run(() => ReplyChannel.Write(Encoding.Unicode.GetBytes(response)));
            return new OutputBase();
        }
    }
}