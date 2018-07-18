using System;
using System.Collections.Generic;
using System.Linq;
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

        public IDisposable LoadingHandle => DiscordSocketMessage.Channel.EnterTypingState();

        public string Content => DiscordSocketMessage.Content;

        public bool TargetsMe => DiscordSocketMessage.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id);

        public async Task SendEmbedAsync(string v1, Embed v3)
        {
            var builder = new Discord.EmbedBuilder();
            foreach (var value in v3)
            {
                builder.AddInlineField(value.Key, value.Value);
            }
            await DiscordSocketMessage.Channel.SendMessageAsync(v1, false, builder.Build());
        }

        public async Task<RestUserMessage> SendFileAsync(string file, string text)
        {
            return await DiscordSocketMessage.Channel.SendFileAsync(file, text);
        }

        public async Task SendMessageAsync(string response)
        {
            await DiscordSocketMessage.Channel.SendMessageAsync(response);
        }
    }
}