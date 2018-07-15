using System;
using Discord.WebSocket;
using System.Collections.Generic;
using BigBeautifulBot.Properties;
using System.Threading.Tasks;
using System.Linq;
using BigBeautifulBot.Input.Inputs;

namespace BigBeautifulBot
{
    public class FoodProcessor : InputProcessorBase<MealInput>
    {
        private string _LastAppetiteComment;

        public FoodProcessor(BigBeautifulBot bot) : base(bot)
        {
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

        public override bool TryParse(IMessage message, out IInput input)
        {
            var foods = new MealInput(message);
            input = foods;
            CommandInput command = null;//TODO: I wish I didn't need to instantiate an input processor here...
            if (!(new CommandProcessor(_Bot)).TryParse(message, out IInput inputCommand) || !(command = (CommandInput)inputCommand).CommandName.Equals("use")) return false;

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
                            foods.FoodContent.Add(food.Value);
                            ok = true;
                        }
                    }

                    if (!ok) return ok;
                } while (stringLeft.Length > 0);
            }
            return true;
        }

        public override async Task Process(MealInput mealInput)
        {
            if (!mealInput.FoodContent.Any()) return;

            foreach (var foodItem in mealInput.FoodContent)
            {
                if (_Bot.Info.IsOverfed)
                {
                    await mealInput.Respond(Resources.OverfedComment);
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
            var responseText = mealInput.FoodContent.Last().BotComment;

            //Append an appetite comment if there's a new one
            var appetiteComment = GetAppetiteComment(_Bot.Info.Appetite);
            if (_LastAppetiteComment != appetiteComment)
            {
                responseText += '\n';
                responseText += appetiteComment;
                _LastAppetiteComment = appetiteComment;
            }

            //Send reponse
            await mealInput.Respond(responseText);
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
    }
}