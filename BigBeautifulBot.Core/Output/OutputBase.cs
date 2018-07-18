using System;
using System.Collections;
using System.Threading.Tasks;
using Discord.Rest;

namespace BigBeautifulBot.Input.Inputs
{
    public class OutputBase
    {
        public RestUserMessage Message {get;}

        public OutputBase(RestUserMessage message)
        {
            Message = message;
        }

        public async Task AddScalarUI(string ui)
        {
            await Message.AddReactionAsync(new Discord.Emoji(ui));
        }

        internal async Task<int> GetScalarUI(string tick)
        {
            var users = await Message.GetReactionUsersAsync(tick);
            return users.Count;
        }
    }
}