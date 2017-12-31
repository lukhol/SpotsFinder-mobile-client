using Plugin.DeviceInfo.Abstractions;
using SpotFinder.DataServices;
using SpotFinder.Models.DTO;
using SpotFinder.Redux.StateModels;
using System;

namespace SpotFinder.Services
{
    public class ErrorLogger : IErrorLogger
    {
        private readonly IDeviceInfo DeviceInfo;
        private readonly IErrorService errorService;

        public ErrorLogger(IDeviceInfo deviceInfo, IErrorService errorService)
        {
            this.DeviceInfo = deviceInfo ?? throw new ArgumentNullException(nameof(deviceInfo));
            this.errorService = errorService ?? throw new ArgumentNullException(nameof(errorService));
        }

        public void LogError(ErrorState errorState)
        {
            var deviceInfo = new DeviceInfo
            {
                Idiom = DeviceInfo.Idiom.ToString(),
                Model = DeviceInfo.Model,
                Platform = DeviceInfo.Platform.ToString(),
                Version = DeviceInfo.Version,
                VersionNumber = DeviceInfo.VersionNumber.ToString()
            };

            var errorInfo = new ErrorInfo()
            {
                DeviceInfo = deviceInfo,
                WhereOccurred = errorState.WhereOccurred,
                ClassName = nameof(errorState.Error),
                Message = errorState.Error.Message,
                StackTraceString = errorState.Error.StackTrace
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
