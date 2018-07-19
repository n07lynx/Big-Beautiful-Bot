using System.Threading.Tasks;

namespace BigBeautifulBot.Input.Inputs
{
    public interface IInput
    {
        Task<OutputBase> Respond(string response);
    }
}