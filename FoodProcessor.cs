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

            if (_Bot.Info.Appetite > Math.Sign(_Bot.Config.OverfeedLimit))
            {
                _Bot.Info.Weight += foodItem.WeightValue;
                _Bot.Info.Appetite -= foodItem.WeightValue;
                await message.Channel.SendMessageAsync(foodItem.BotComment);
                //TODO: Comments as her appetite goes down
            }
            else
            {
                await message.Channel.SendMessageAsync(Resources.OverfedComment);
            }
        }

        public static readonly Dictionary<string, FoodInfo> Definitions = new Dictionary<string, FoodInfo>
        {
            { "🍕", new FoodInfo(0.05M, Resources.UsePizza) },
            { "🍮", new FoodInfo(0.22M, Resources.UseCustard) },
            { "🥞", new FoodInfo(0.19M, Resources.UsePancake) },
            { "🍰", new FoodInfo(0.2M, Resources.UseShortcake) },
            { "🍇", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍈", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍉", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍊", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍋", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍌", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍍", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍎", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍏", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍐", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍑", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍒", new FoodInfo(3.0M, Resources.UseCherry) },
            { "🍓", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥝", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍅", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥥", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥑", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍆", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥔", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥕", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🌽", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🌶", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥒", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥦", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍄", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥜", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🌰", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍞", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥐", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥖", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥨", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🧀", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍖", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍗", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥩", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥓", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍔", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍟", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🌭", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥪", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🌮", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🌯", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍳", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍲", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥣", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥗", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍿", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥫", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍱", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍘", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍙", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍚", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍛", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍜", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍝", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍠", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍢", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍣", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍤", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍥", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍡", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥟", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥠", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥡", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍦", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍧", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍨", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍩", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍪", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🎂", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥧", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍫", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍬", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍭", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍯", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍼", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥛", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "☕", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍵", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍶", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍾", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍷", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍸", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍹", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍺", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍻", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥂", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥃", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥤", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "<:cupcake:409416270534934529>", new FoodInfo(0.15M, Resources.UseCupcake) },
        };
    }
}