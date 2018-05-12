using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using BigBeautifulBot.Properties;

namespace BigBeautifulBot
{
    public class Scales
    {
        private BigBeautifulBot _Bot;

        public Scales(BigBeautifulBot bigBeautifulBot)
        {
            _Bot = bigBeautifulBot;
        }

        internal async Task PerformWeighIn(SocketMessage message)
        {
            var weight = _Bot.Info.Weight;
            var response = string.Format(Resources.UseScales, weight) + '\n';
            response += GetWeightCommend(weight, true);
            await message.Channel.SendMessageAsync(response);
        }

        public static string GetWeightCommend(decimal weight, bool self)
        {
            if (self)
            {
                if (weight > 160) return Resources.SelfWeightCommentGT150;
                else if (weight > 150) return Resources.SelfWeightComment150;
                else if (weight > 140) return Resources.SelfWeightComment140;
                else if (weight > 130) return string.Format(Resources.SelfWeightComment130, weight);
                else if (weight > 120) return Resources.SelfWeightComment120;
                else if (weight > 110) return Resources.SelfWeightComment110;
                else if (weight > 100) return string.Format(Resources.SelfWeightComment100, weight);
                else if (weight > 90) return Resources.SelfWeightComment90;
                else if (weight > 80) return Resources.SelfWeightComment80;
                else if (weight > 70) return Resources.SelfWeightComment70;
                else if (weight > 60) return Resources.SelfWeightComment60;
                else return Resources.SelfWeightCommentLT60;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}