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
        Task<OutputBase> SendMessageAsync(string response);
        Task<OutputBase> SendFileAsync(string file, string text);
        Task SendEmbedAsync(string v1, Embed v3);
    }
}