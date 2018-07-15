using System.Threading.Tasks;
using Discord.WebSocket;

namespace BigBeautifulBot
{
    public abstract class InputProcessorBase
    {
        protected BigBeautifulBot _Bot;

        public InputProcessorBase(BigBeautifulBot bot)
        {
            _Bot = bot;
        }

        public abstract Task Process(SocketMessage message);
    }
}