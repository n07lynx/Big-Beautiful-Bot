using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BigBeautifulBot.Properties;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Processors
{
    public class LanguageProcessor : InputProcessorBase
    {
        public LanguageProcessor(BigBeautifulBot bot) : base(bot)
        {
        }

   Dictionary<string, string> _ResponseMap = new Dictionary<string, string>
        {
            { Resources.RegexGreeting, Resources.MentionGreeting },
            { Resources.RegexWhoIs, Resources.MentionWhoIs},
            { Resources.RegexGoodnight, Resources.MentionGoodnight },
            { Resources.RegexBully, Resources.MentionBully },
            { Resources.RegexLove, Resources.MentionLove }
        };

        public override async Task  Process(SocketMessage message)
        {
  if (message.MentionedUsers.Any(x => x.Id == Program.client.CurrentUser.Id))//Mention (TODO: Move to a language parser class)
                {
                    var messageContent = message.Content;
                    if (message.Author.ToString() == BBBInfo.TheCreator)//Admin instructions
                    {
                        if (Regex.IsMatch(messageContent, Resources.RegexThatsRight, RegexOptions.IgnoreCase))
                        {
                            await message.Channel.SendMessageAsync(Resources.MentionThatsRight);
                            return;
                        }
                        else if (Regex.IsMatch(messageContent, Resources.RegexFalseAlarm, RegexOptions.IgnoreCase))
                        {
                            await message.Channel.SendMessageAsync(Resources.MentionFalseAlarm);
                            return;
                        }
                    }

                    foreach (var response in _ResponseMap) //Regular mention request/responses
                    {
                        if (Regex.IsMatch(messageContent, response.Key, RegexOptions.IgnoreCase))
                        {
                            await message.Channel.SendMessageAsync(string.Format(response.Value, message.Author.Mention));
                            return;
                        }
                    }

                    //Fallback message
                    await message.Channel.SendMessageAsync(Resources.MentionUnknown);
                }        }
    }
}