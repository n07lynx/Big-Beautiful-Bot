namespace BigBeautifulBot
{
    public class FoodInfo
    {
        public FoodInfo(decimal weightValue, string comment)
        {
            WeightValue = weightValue;
            BotComment = comment;
        }

        public string BotComment { get; internal set; }
        public decimal WeightValue { get; internal set; }
    }
}