using SpotFinder.Redux.StateModels;

namespace SpotFinder.Services
{
    public interface IErrorLogger
    {
        void LogError(ErrorState errorState);
    }
}
