using System;
using System.Runtime.Serialization;

namespace BigBeautifulBot.Output
{
    [Serializable]
    public class BBBException : Exception
    {
        public string UserMessage { get; }
        public BBBException(string userMessage)
        {
            UserMessage = userMessage;
        }
    }
}