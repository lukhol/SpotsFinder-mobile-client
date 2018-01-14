using Plugin.DeviceInfo.Abstractions;
using SpotFinder.DataServices;
using SpotFinder.Redux.StateModels;
using System;
using System.Net;

namespace SpotFinder.Redux.Actions.WrongPlaceReports
{
    public class SetWrongPlaceReportActionCreator : ISetWrongPlaceReportActionCreator
    {
        private IWrongPlaceReportService WrongPlaceReportService;
        private IDeviceInfo DeviceInfo;

        public SetWrongPlaceReportActionCreator(IDeviceInfo deviceInfo, IWrongPlaceReportService wrongPlaceReportService)
        {
            WrongPlaceReportService = wrongPlaceReportService ?? throw new ArgumentNullException(nameof(wrongPlaceReportService));
            DeviceInfo = deviceInfo ?? throw new ArgumentNullException(nameof(deviceInfo));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> UploadWrongPlaceReport(string reasonComment, bool isNotSkateboardPlace)
        {
            return async (dispatch, getState) =>
            {
                var state = getState();
                long userId = GetUserId(state.UserState.User);

                var wrongPlaceReport = new WrongPlaceReport(
                    placeId: state.PlacesData.CurrentPlaceState.Value.Id,
                    placeVersion: state.PlacesData.CurrentPlaceState.Value.Version,
                    userId: userId,
                    deviceId: DeviceInfo.Id,
                    reasonComment: reasonComment,
                    isNotSkateboardPlace: isNotSkateboardPlace
                );

                dispatch(new SetWrongPlaceReportStartAction(wrongPlaceReport));

                try
                {
                    await WrongPlaceReportService.SendAsync(wrongPlaceReport);
                    dispatch(new SetWrongPlaceReportCompleteAction(wrongPlaceReport));
                }
                catch (Exception e)
                {
                    dispatch(new SetWrongPlaceReportCompleteAction(e));
                }
            };
        }

        private long GetUserId(User user)
        {
            if (user != null)
                return user.Id;
            else
                return 0;
        }
    }
}
