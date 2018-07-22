using System.Threading.Tasks;
using BigBeautifulBot.Output;

namespace BigBeautifulBot.Input
{
    public interface IInput
    {
        Task<OutputBase> Respond(string response);
    }
}