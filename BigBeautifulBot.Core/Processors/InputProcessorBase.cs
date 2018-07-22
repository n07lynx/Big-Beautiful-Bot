using System.Threading.Tasks;
using BigBeautifulBot.Input;
using BigBeautifulBot.Input.Processors;
using Discord.WebSocket;

namespace BigBeautifulBot
{
    public abstract class InputProcessorBase<T> : IInputProcessor where T : IInput
    {
        protected BigBeautifulBot _Bot;

        public InputProcessorBase(BigBeautifulBot bot)
        {
            _Bot = bot;
        }

        public abstract Task Process(T input);

        public async Task Process(IInput input) => await Process((T)input);

        public abstract bool TryParse(IMessage message, out IInput input);
    }
}