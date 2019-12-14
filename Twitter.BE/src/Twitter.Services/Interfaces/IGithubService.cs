using System.Threading.Tasks;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface IGithubService
    {
        Task<IBaseResponse> CreateGistURL(GistRequest gist, string text, int currentUserId);
        Task<IBaseResponse> AddToken(string token, int currentUserId);
        Task<IBaseResponse> RemoveToken(int currentUserId);
    }
}
