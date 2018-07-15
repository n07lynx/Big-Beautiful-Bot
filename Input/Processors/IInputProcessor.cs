using System.Threading.Tasks;
using BigBeautifulBot.Input.Inputs;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Processors
{
    public interface IInputProcessor
    {
        Task Process(IInput input);
        bool TryParse(SocketMessage message, out IInput input);
    }
}