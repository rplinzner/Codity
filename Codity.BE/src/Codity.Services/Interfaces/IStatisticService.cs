using System.Threading.Tasks;

namespace Codity.Services.Interfaces
{
    public interface IStatisticService
    {
        Task SendWeeklyStatisticSummary();
    }
}
