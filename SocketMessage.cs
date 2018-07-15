using System;
using System.Collections.Generic;
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

        public UserIdentity Author {get;}
        public string Content { get; internal set; }

        public List<UserIdentity> MentionedUsers => throw new NotImplementedException();

        public IDisposable LoadingHandle => throw new NotImplementedException();

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