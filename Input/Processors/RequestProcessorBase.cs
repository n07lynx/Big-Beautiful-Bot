using System.Threading.Tasks;
using Discord.WebSocket;

namespace BigBeautifulBot
{
    public abstract class RequestProcessorBase
    {
        protected BigBeautifulBot _Bot;

        public RequestProcessorBase(BigBeautifulBot bot)
        {
            _Bot = bot;
        }

        public abstract Task Process(SocketMessage message);
    }
}