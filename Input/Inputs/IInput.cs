using System.Threading.Tasks;

namespace BigBeautifulBot.Input.Inputs
{
    public interface IInput
    {
        Task Respond(string response);
    }
}