using System.Threading.Tasks;

namespace Twitter.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmail(string receiver, string title, string message);
    }
}
