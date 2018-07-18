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
        IDisposable LoadingHandle { get; }
        string Content { get; }
        bool TargetsMe { get; }
        Task SendMessageAsync(string response);
        Task<RestUserMessage> SendFileAsync(string file, string text);
        Task SendEmbedAsync(string v1, Embed v3);
    }
}