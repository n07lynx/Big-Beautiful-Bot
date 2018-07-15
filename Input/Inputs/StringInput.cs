using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Inputs
{
    public class StringInput : InputBase
    {
        public StringInput(SocketMessage message) : base(message)
        {
        }

        public string Text => Message.Content;
    }
}