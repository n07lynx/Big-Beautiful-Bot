using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BigBeautifulBot.Properties;
using Discord.WebSocket;

namespace BigBeautifulBot.Input.Inputs
{
    public class MealInput : InputBase
    {
        public List<FoodInfo> FoodContent = new List<FoodInfo>();

        public MealInput(IMessage message) : base (message)
        {
        }
    }
}