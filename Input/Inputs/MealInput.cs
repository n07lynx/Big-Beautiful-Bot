using System;
using System.Collections.Generic;
using BigBeautifulBot.Properties;

namespace BigBeautifulBot.Input.Inputs
{
    public class MealInput : List<FoodInfo>, IInput
    {
        public static bool TryParse(Discord.WebSocket.SocketMessage message, BBBSettings settings, out List<FoodInfo> foods)
        {
            foods = new List<FoodInfo>();
            if (!CommandInput.TryParse(message, settings, out var command) || !command.CommandName.Equals("use")) return false;

            foreach (var arg in command.Args)
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

        public static readonly Dictionary<string, FoodInfo> Definitions = new Dictionary<string, FoodInfo>
        {
            { "🍕", new FoodInfo(0.05M, Resources.UsePizza) },
            { "🍮", new FoodInfo(0.22M, Resources.UseCustard) },
            { "🥞", new FoodInfo(0.19M, Resources.UsePancake) },
            { "🍰", new FoodInfo(0.3M, Resources.UseShortcake) },
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
            { "🍟", new FoodInfo(0.3M, Resources.UseFoodUnknown) },
            { "🌭", new FoodInfo(0.2M, Resources.UseFoodUnknown) },
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
            { "🍩", new FoodInfo(0.5M, Resources.UseFoodUnknown) },
            { "🍪", new FoodInfo(0.2M, Resources.UseFoodUnknown) },
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
            { "<:labombe:450987114557865995>", new FoodInfo(2.8M, Resources.UseLabombe) }
        };
    }
}