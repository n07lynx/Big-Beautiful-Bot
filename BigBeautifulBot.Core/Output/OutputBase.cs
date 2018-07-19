using System;
using System.Collections;
using System.Threading.Tasks;
using Discord.Rest;

namespace BigBeautifulBot.Input.Inputs
{
    public class OutputBase
    {
        //TODO: Move to derrived class
        public RestUserMessage Message { get; }

        public OutputBase(RestUserMessage message)
        {
            Message = message;
        }

        public OutputBase()
        {
        }

        public async Task AddScalarUI(string ui)
        {
            if (Message == null) throw new NotImplementedException("TODO: UI for socket clients");
            await Message.AddReactionAsync(new Discord.Emoji(ui));
        }

        internal async Task<int> GetScalarUI(string tick)
        {
            if (Message == null) throw new NotImplementedException("TODO: UI for socket clients");
            var users = await Message.GetReactionUsersAsync(tick);
            return users.Count;
        }

        internal Task PresentChoice(params string[] v1)
        {
            Program.client.ReactionAdded
        }
    }
}