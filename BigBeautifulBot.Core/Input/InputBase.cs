using System;
using System.Linq;
using System.Threading.Tasks;
using BigBeautifulBot.Output;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Inputs
{
    //TODO: Move discord-specific code into an interfaced wrapper
    public class InputBase : IInput
    {
        public InputBase(IMessage message)
        {
            Message = message;
            Author = message.Author;
        }

        public IMessage Message { get; private set; }

        public bool TargetsMe => Message.TargetsMe;

        public UserIdentity Author {get;set;}

        public IDisposable LoadingHandle => Message.LoadingHandle;

        public async Task<OutputBase> Respond(string response)
        {
            return await Message.SendMessageAsync(response);
        }

        public async Task<OutputBase> FileRespond(string file, string text = null)
        {
            return await Message.SendFileAsync(file, text);
        }
    }
}