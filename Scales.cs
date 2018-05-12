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

            if (weight > 160) response += Resources.SelfWeightCommentGT150;
            else if (weight > 150) response += Resources.SelfWeightComment150;
            else if (weight > 140) response += Resources.SelfWeightComment140;
            else if (weight > 130) response += string.Format(Resources.SelfWeightComment130, weight);
            else if (weight > 120) response += Resources.SelfWeightComment120;
            else if (weight > 110) response += Resources.SelfWeightComment110;
            else if (weight > 100) response += string.Format(Resources.SelfWeightComment100, weight);
            else if (weight > 90) response += Resources.SelfWeightComment90;
            else if (weight > 80) response += Resources.SelfWeightComment80;
            else if (weight > 70) response += Resources.SelfWeightComment70;
            else if (weight > 60) response += Resources.SelfWeightComment60;
            else response += Resources.SelfWeightCommentLT60;

            await message.Channel.SendMessageAsync(response);
        }
    }
}