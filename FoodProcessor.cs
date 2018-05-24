using System;
using Discord.WebSocket;
using System.Collections.Generic;
using BigBeautifulBot.Properties;
using System.Threading.Tasks;
using System.Linq;

namespace BigBeautifulBot
{
    public class FoodProcessor
    {
        private string _LastAppetiteComment;

        private BigBeautifulBot _Bot;

        public FoodProcessor(BigBeautifulBot bot)
        {
            _Bot = bot;
        }

        internal async Task Consume(IEnumerable<FoodInfo> foodItems, SocketMessage message)
        {
            foreach (var foodItem in foodItems)
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
            var responseText = foodItems.Last().BotComment;

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
            else if (appetite >= 10) return string.Format(Resources.AppetiteComment10, Program.GetRandomElement(Definitions.Keys.ToArray()));
            else if (appetite >= 9) return Resources.AppetiteComment9;
            else if (appetite >= 8) return Resources.AppetiteComment8;
            else if (appetite >= 7) return Resources.AppetiteComment7;
            else if (appetite >= 6) return Resources.AppetiteComment6;
            else if (appetite >= 5) return string.Format(Resources.AppetiteComment5, Program.GetRandomElement(Definitions.Keys.ToArray()));
            else if (appetite >= 4) return Resources.AppetiteComment4;
            else if (appetite >= 3) return Resources.AppetiteComment3;
            else if (appetite >= 2) return Resources.AppetiteComment2;
            else if (appetite >= 1) return Resources.AppetiteComment1;
            else if (appetite >= 0) return Resources.AppetiteComment0;
            else return Resources.AppetiteCommentFull;
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
            { "🥖", new FoodInfo(0.2M, Resources.UseFoodUnknown) },
            { "🥨", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🧀", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍖", new FoodInfo(0.3M, Resources.UseFoodUnknown) },
            { "🍗", new FoodInfo(0.2M, Resources.UseFoodUnknown) },
            { "🥩", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🥓", new FoodInfo(0.4M, Resources.UseFoodUnknown) },
            { "🍔", new FoodInfo(0.6M, Resources.UseFoodUnknown) },
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
            { "🎂", new FoodInfo(1M, Resources.UseFoodUnknown) },
            { "🥧", new FoodInfo(0.1M, Resources.UseFoodUnknown) },
            { "🍫", new FoodInfo(0.4M, Resources.UseFoodUnknown) },
            { "🍬", new FoodInfo(0.2M, Resources.UseFoodUnknown) },
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

        internal bool TryParseFoods(string[] args, out List<FoodInfo> foods)
        {
            foods = new List<FoodInfo>();
            foreach (var arg in args)
            {
                var stringLeft = arg;
                do
                {
                    var ok = false;
                    foreach (var food in Definitions)
                    {
                        if (stringLeft.StartsWith(food.Key, StringComparison.InvariantCultureIgnoreCase))
                        {
                            stringLeft = stringLeft.Substring(food.Key.Length);
                            foods.Add(food.Value);
                            ok = true;
                        }
                    }

                    if (!ok) return ok;
                } while (stringLeft.Length > 0);
            }
            return true;
        }
    }
}