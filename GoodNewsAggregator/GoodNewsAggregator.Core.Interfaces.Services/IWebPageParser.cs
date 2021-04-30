using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IWebPageParser // Parse - get raw data -> requested data 
    {
        Task<string> Parse(string url);
    }
}