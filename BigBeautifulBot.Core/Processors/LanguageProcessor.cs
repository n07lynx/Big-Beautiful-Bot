using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BigBeautifulBot.Input.Inputs;
using BigBeautifulBot.Properties;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Processors
{
    public class LanguageProcessor : InputProcessorBase<StringInput>
    {
        public LanguageProcessor(BigBeautifulBot bot) : base(bot)
        {
        }

        //TODO: Move to database
        Dictionary<string, string> _ResponseMap = new Dictionary<string, string>
        {
            { Resources.RegexGreeting, Resources.MentionGreeting },
            { Resources.RegexWhoIs, Resources.MentionWhoIs},
            { Resources.RegexGoodnight, Resources.MentionGoodnight },
            { Resources.RegexBully, Resources.MentionBully },
            { Resources.RegexLove, Resources.MentionLove }
        };

        public override async Task Process(StringInput message)
        {
            if (message.TargetsMe)
            {
                if (message.Author.IsAdmin)//Admin instructions
                {
                    if (Regex.IsMatch(message.Text, Resources.RegexThatsRight, RegexOptions.IgnoreCase))
                    {
                        await message.Respond(Resources.MentionThatsRight);
                        return;
                    }
                    else if (Regex.IsMatch(message.Text, Resources.RegexFalseAlarm, RegexOptions.IgnoreCase))
                    {
                        await message.Respond(Resources.MentionFalseAlarm);
                        return;
                    }
                }

                foreach (var response in _ResponseMap) //Regular mention request/responses
                {
                    if (Regex.IsMatch(message.Text, response.Key, RegexOptions.IgnoreCase))
                    {
                        await message.Respond(string.Format(response.Value, message.Author));
                        return;
                    }
                }

                //Fallback message
                await message.Respond(Resources.MentionUnknown);
            }
        }

        public override bool TryParse(IMessage message, out IInput input)
        {
            input = new StringInput(message);
            return true;
        }
    }
}