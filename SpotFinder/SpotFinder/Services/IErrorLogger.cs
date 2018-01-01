using SpotFinder.Redux.StateModels;
using System.Threading.Tasks;

namespace SpotFinder.Services
{
    public interface IErrorLogger
    {
        Task LogErrorAsync(ErrorState errorState);
    }
}
