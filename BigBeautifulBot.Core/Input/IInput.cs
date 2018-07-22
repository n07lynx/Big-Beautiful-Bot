using System.Threading.Tasks;
using BigBeautifulBot.Output;

namespace BigBeautifulBot.Input.Inputs
{
    public interface IInput
    {
        Task<OutputBase> Respond(string response);
    }
}