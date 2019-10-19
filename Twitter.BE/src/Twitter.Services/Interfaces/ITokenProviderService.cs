using Twitter.Data.Model;

namespace Twitter.Services.Interfaces
{
    public interface ITokenProviderService
    {
        string GenerateToken(User user);
    }
}
