using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Inputs
{
    //TODO: Move discord-specific code into an interfaced wrapper
    public class InputBase : IInput
    {
        public InputBase(SocketMessage message)
        {
            Message = message;
            Author = new UserIdentity(message.Author);
        }

        public SocketMessage Message { get; private set; }

        public bool TargetsMe => Message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id);

        public UserIdentity Author {get;set;}

        public IDisposable LoadingHandle => Message.Channel.EnterTypingState();

        public async Task Respond(string response)
        {
            await Message.Channel.SendMessageAsync(response);
        }

        public async Task<OutputBase> FileRespond(string file, string text = null)
        {
            return new OutputBase(await Message.Channel.SendFileAsync(file, text));
        }
    }
}