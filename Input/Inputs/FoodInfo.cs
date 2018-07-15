using System;

namespace BigBeautifulBot.Input.Inputs
{
    public class FoodInfo
    {
        public FoodInfo(decimal weightValue, string comment)
        {
            if (string.IsNullOrEmpty(comment)) throw new ArgumentNullException(nameof(comment));
            WeightValue = weightValue;
            BotComment = comment;
        }

        public string BotComment { get; internal set; }
        public decimal WeightValue { get; internal set; }
    }
}