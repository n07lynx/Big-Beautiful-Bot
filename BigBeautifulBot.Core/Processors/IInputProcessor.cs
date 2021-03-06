using System.Threading.Tasks;
using BigBeautifulBot.Input;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Processors
{
    public interface IInputProcessor
    {
        Task Process(IInput input);
        bool TryParse(IMessage message, out IInput input);
    }
}