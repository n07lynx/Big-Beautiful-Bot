using System.Linq;
using Discord.WebSocket;

namespace BigBeautifulBot
{
    public class Command
    {
        public string[] Args { get; internal set; }
        public string CommandName { get; internal set; }

        public static bool TryParse(SocketMessage message, BBBSettings settings, out Command command)
        {
            var messageContent = message.Content;
            if (messageContent.StartsWith(settings.Prefix))
            {
                var components = new string(messageContent.Skip(settings.Prefix.Length).ToArray()).Trim().Split(' ');
                var commandName = components.First().ToLower();
                var args = components.Skip(1).ToArray();
                command = new Command { CommandName = commandName, Args = args };
                return true;
            }
            else
            {
                command = null;
                return false;
            }
        }
    }
}