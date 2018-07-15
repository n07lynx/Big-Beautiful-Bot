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
        }

        public SocketMessage Message { get; private set; }

        public bool TargetsMe => Message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id);

        public bool IsAdmin => Author.ToString() == BBBInfo.TheCreator;

        public string Author => Message.Author.ToString();

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