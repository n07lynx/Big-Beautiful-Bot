using System;
using Discord.WebSocket;
using System.Collections.Generic;
using BigBeautifulBot.Properties;
using System.Threading.Tasks;
using System.Linq;
using BigBeautifulBot.Input.Inputs;

namespace BigBeautifulBot
{
    public class FoodProcessor : InputProcessorBase
    {
        private string _LastAppetiteComment;

        public FoodProcessor(BigBeautifulBot bot) : base(bot)
        {
        }

        public override async Task Process(SocketMessage message)
        {
            if(!CommandInput.TryParse(message, _Bot.Config, out var command) || !MealInput.TryParse(message, _Bot.Config, out var foods) || !foods.Any()) return;

            foreach (var foodItem in foods)
            {
                if (_Bot.IsOverfed)
                {
                    await message.Channel.SendMessageAsync(Resources.OverfedComment);
                    return;
                }
                else
                {
                    //Apply affects
                    _Bot.Info.Weight += foodItem.WeightValue;
                    _Bot.Info.Appetite -= foodItem.WeightValue;
                }
            }

            //Get response
            var responseText = foods.Last().BotComment;

            //Append an appetite comment if there's a new one
            var appetiteComment = GetAppetiteComment(_Bot.Info.Appetite);
            if (_LastAppetiteComment != appetiteComment)
            {
                responseText += '\n';
                responseText += appetiteComment;
                _LastAppetiteComment = appetiteComment;
            }

            //Send reponse
            await message.Channel.SendMessageAsync(responseText);
        }

        private string GetAppetiteComment(decimal appetite)
        {
            if (appetite >= 11) return Resources.AppetiteCommentHungry;
            else if (appetite >= 10) return string.Format(Resources.AppetiteComment10, Program.GetRandomElement(MealInput.Definitions.Keys.ToArray()));
            else if (appetite >= 9) return Resources.AppetiteComment9;
            else if (appetite >= 8) return Resources.AppetiteComment8;
            else if (appetite >= 7) return Resources.AppetiteComment7;
            else if (appetite >= 6) return Resources.AppetiteComment6;
            else if (appetite >= 5) return string.Format(Resources.AppetiteComment5, Program.GetRandomElement(MealInput.Definitions.Keys.ToArray()));
            else if (appetite >= 4) return Resources.AppetiteComment4;
            else if (appetite >= 3) return Resources.AppetiteComment3;
            else if (appetite >= 2) return Resources.AppetiteComment2;
            else if (appetite >= 1) return Resources.AppetiteComment1;
            else if (appetite >= 0) return Resources.AppetiteComment0;
            else return Resources.AppetiteCommentFull;
        }
    }
}