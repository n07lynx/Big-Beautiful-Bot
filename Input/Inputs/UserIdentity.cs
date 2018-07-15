using Discord.WebSocket;

namespace BigBeautifulBot.Input.Inputs
{
    public struct UserIdentity
    {
        public UserIdentity(SocketUser user)
        {
            UserName = user.ToString();
            SystemName = user.Mention;
            IsAdmin = UserName == BBBInfo.TheCreator;
        }

        public string UserName {get;}
        public string SystemName {get;}
        public bool IsAdmin {get;}
    }
}