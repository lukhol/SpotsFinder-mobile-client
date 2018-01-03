using SpotFinder.Redux.StateModels;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IWrongPlaceReportService
    {
        Task SendAsync(WrongPlaceReport wrongPlaceReport);
    }
}
