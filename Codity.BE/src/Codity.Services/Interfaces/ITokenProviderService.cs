using Codity.Data.Model;

namespace Codity.Services.Interfaces
{
    public interface ITokenProviderService
    {
        string GenerateToken(User user);
    }
}
