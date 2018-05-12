using System;
using Discord.WebSocket;
using System.Collections.Generic;
using BigBeautifulBot.Properties;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    public class FoodProcessor
    {
        private BigBeautifulBot _Bot;

        public FoodProcessor(BigBeautifulBot bot)
        {
            _Bot = bot;
        }

        internal async Task Consume(string itemCode, SocketMessage message)
        {
            var foodItem = Definitions[itemCode];
            _Bot.Info.Weight += foodItem.WeightValue;
            await message.Channel.SendMessageAsync(foodItem.BotComment);
        }

        public static readonly Dictionary<string, FoodInfo> Definitions = new Dictionary<string, FoodInfo>
        {
            { "🍕", new FoodInfo(0.05M, Resources.UsePizza) }
        };
    }
}