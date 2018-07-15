using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BigBeautifulBot.Input.Inputs;
using Discord.Rest;

namespace BigBeautifulBot
{
    public interface IMessage
    {
        UserIdentity Author { get; }
        List<UserIdentity> MentionedUsers { get; }
        IDisposable LoadingHandle { get; }
        string Content { get; }

        Task SendMessageAsync(string response);
        Task<RestUserMessage> SendFileAsync(string file, string text);
        Task SendEmbedAsync(string v1, bool v2, object v3);
    }
}