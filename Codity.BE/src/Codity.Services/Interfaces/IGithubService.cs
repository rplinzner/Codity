using System.Threading.Tasks;
using Codity.Services.RequestModels.Post;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface IGithubService
    {
        Task<IBaseResponse> CreateGistURL(GistRequest gist, string text, int currentUserId);
        Task<IBaseResponse> AddToken(string token, int currentUserId);
        Task<IBaseResponse> RemoveToken(int currentUserId);
    }
}
