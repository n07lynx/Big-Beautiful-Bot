using System.Linq;
using Discord.WebSocket;

namespace BigBeautifulBot
{
    public class Command
    {
        public string[] Args { get; internal set; }
        public string CommandName { get; internal set; }

        public static Command GetCommand(SocketMessage message, BBBSettings settings)
        {
            var messageContent = message.Content;
            if (messageContent.StartsWith(settings.Prefix))
            {
                var components = new string(messageContent.Skip(settings.Prefix.Length).ToArray()).Trim().Split(' ');
                var command = components.First().ToLower();
                var args = components.Skip(1).ToArray();
                return new Command { CommandName = command, Args = args };
            }
            else
            {
                return null;
            }
        }
    }
}