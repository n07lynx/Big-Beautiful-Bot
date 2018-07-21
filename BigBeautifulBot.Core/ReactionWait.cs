using System.Threading.Tasks;
using Discord.Rest;

namespace BigBeautifulBot
{
    public class ReactionWait
    {
        public TaskCompletionSource<string> CompletionSource { get; }
        public string[] Options { get; }
        public ulong Message { get; }
        public string ExclusiveInputUser { get; }

        public ReactionWait(TaskCompletionSource<string> completionSource, string[] option, ulong messageId, string exclusiveInputUser)
        {
            this.CompletionSource = completionSource;
            this.Options = option;
            this.Message = messageId;
            this.ExclusiveInputUser = exclusiveInputUser;
        }
    }
}