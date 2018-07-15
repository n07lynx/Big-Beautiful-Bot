using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BigBeautifulBot.Input.Inputs;
using Discord.Rest;
using Discord.WebSocket;

namespace BigBeautifulBot
{
    internal class DiscordMessageWrapper : IMessage
    {
        private SocketMessage DiscordSocketMessage;

        public DiscordMessageWrapper(SocketMessage socketMessage)
        {
            DiscordSocketMessage = socketMessage;
            Author = new UserIdentity(socketMessage.Author);
        }

        public UserIdentity Author { get; }

        public List<UserIdentity> MentionedUsers => throw new NotImplementedException();

        public IDisposable LoadingHandle => throw new NotImplementedException();

        public string Content => throw new NotImplementedException();

        public Task SendEmbedAsync(string v1, bool v2, object v3)
        {
            throw new NotImplementedException();
        }

        public Task<RestUserMessage> SendFileAsync(string file, string text)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageAsync(string response)
        {
            throw new NotImplementedException();
        }
    }
}