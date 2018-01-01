using Moq;
using NUnit.Framework;
using Plugin.DeviceInfo.Abstractions;
using SpotFinder.DataServices;
using SpotFinder.Models.DTO;
using SpotFinder.Redux.StateModels;
using SpotFinder.Services;
using System;

namespace SpotFinder.TestTwo
{
    [TestFixture]
    public class ErrorLoggerTests
    {
        private Mock<IDeviceInfo> mockDeviceInfo = new Mock<IDeviceInfo>();
        private Mock<IErrorService> mockErrorService = new Mock<IErrorService>();

        [Test]
        public void CanLogError()
        {
            var errorLogger = new ErrorLogger(mockDeviceInfo.Object, mockErrorService.Object);
            var exceptionThatOccured = new Exception();
            var mockErrorState = new ErrorState(exceptionThatOccured, "Inside some component.");

            mockDeviceInfo.SetupGet(x => x.Idiom).Returns(Idiom.Phone);
            mockDeviceInfo.SetupGet(x => x.Model).Returns("Model");
            mockDeviceInfo.SetupGet(x => x.Platform).Returns(Platform.Android);
            mockDeviceInfo.SetupGet(x => x.Version).Returns("Version");
            mockDeviceInfo.SetupGet(x => x.VersionNumber).Returns(new Version("8.0"));

            var deviceInfo = new DeviceInfo
            {
                Idiom = "Phone",
                Model = "Model",
                Platform = "Android",
                Version = "Version",
                VersionNumber = "8.0"
            };

            var errorInfo = new ErrorInfo
            {
                WhereOccurred = mockErrorState.WhereOccurred,
                ClassName = nameof(mockErrorState.Error),
                DeviceInfo = deviceInfo,
                Message = mockErrorState.Error.Message,
                StackTraceString = mockErrorState.Error.StackTrace
            };

            errorLogger.LogErrorAsync(mockErrorState);

            mockErrorService.Verify(errorService => errorService.SendErrorInformationAsync(
                It.Is<ErrorInfo>(x =>
                    x.ClassName == errorInfo.ClassName &&
                    x.Message == errorInfo.Message &&
                    x.StackTraceString == errorInfo.StackTraceString &&
                    x.WhereOccurred == errorInfo.WhereOccurred &&
                    x.DeviceInfo.Idiom.Equals(errorInfo.DeviceInfo.Idiom) &&
                    x.DeviceInfo.Model.Equals(errorInfo.DeviceInfo.Model) &&
                    x.DeviceInfo.Platform.Equals(errorInfo.DeviceInfo.Platform) &&
                    x.DeviceInfo.Version.Equals(errorInfo.DeviceInfo.Version) &&
                    x.DeviceInfo.VersionNumber.Equals(errorInfo.DeviceInfo.VersionNumber))
                ),
                Times.Once()
            );
        }
    }
}
