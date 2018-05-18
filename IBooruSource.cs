using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    public interface IBooruSource
    {
        Task<IEnumerable<string>> Search(params string[] @params);
    }
}