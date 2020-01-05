using System.Threading.Tasks;

namespace Twitter.Services.Interfaces
{
    public interface IStatisticService
    {
        Task SendWeeklyStatisticSummary();
    }
}
