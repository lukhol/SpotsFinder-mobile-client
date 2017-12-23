

namespace SpotFinder.Services
{
    public interface IDeviceLocationProvider
    {
        bool IsLoopActivated { get; }
        bool IsAcquiring { get; }
        void RequestDeviceLocationLoopAsync();
        void RequestDeviceLocationOnesAsync();
        void RequestDeviceLocationForReportAsync();
    }
}
