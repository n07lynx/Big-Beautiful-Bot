using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Inputs
{
    public class CommandInput : StringInput
    {
        public CommandInput(IMessage message) : base (message)
        {
        }

        public string[] Args { get; internal set; }
        public string CommandName { get; internal set; }
    }
}