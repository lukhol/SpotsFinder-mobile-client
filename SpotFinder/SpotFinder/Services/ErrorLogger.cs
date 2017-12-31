using Plugin.DeviceInfo.Abstractions;
using SpotFinder.DataServices;
using SpotFinder.Models.DTO;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Services
{
    public class ErrorLogger : IErrorLogger
    {
        private readonly IDeviceInfo deviceInfo;
        private readonly IErrorService errorService;

        public ErrorLogger(IDeviceInfo deviceInfo, IErrorService errorService)
        {
            this.deviceInfo = deviceInfo ?? throw new ArgumentNullException(nameof(deviceInfo));
            this.errorService = errorService ?? throw new ArgumentNullException(nameof(errorService));
        }

        public void LogError(ErrorState errorState)
        {
            var errorInfo = new ErrorInfo()
            {
                Exception = errorState.Error,
                WhereOccurred = errorState.WhereOccurred,
                Idiom = deviceInfo.Idiom.ToString(),
                Model = deviceInfo.Model,
                Platform = deviceInfo.Platform.ToString(),
                Version = deviceInfo.Version,
                VersionNumber = deviceInfo.VersionNumber.ToString()
            };

            try
            {
                errorService.SendErrorInformation(errorInfo);
            }
            catch(Exception e)
            {
                //Log...
            }
        }
    }
}
