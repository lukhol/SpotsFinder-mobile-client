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
                var wrongPlaceReport = new WrongPlaceReport(
                    placeId: state.PlacesData.CurrentPlaceState.Value.Id,
                    placeVersion: state.PlacesData.CurrentPlaceState.Value.Version,
                    userId: 0,
                    deviceId: DeviceInfo.Id,
                    reasonComment: reasonComment,
                    isNotSkateboardPlace: isNotSkateboardPlace
                );

                dispatch(new SetWrongPlaceReportStartAction(wrongPlaceReport));

                try
                {
                    var result = await WrongPlaceReportService.SendAsync(wrongPlaceReport);

                    if (result)
                        dispatch(new SetWrongPlaceReportCompleteAction(wrongPlaceReport));
                    else
                        throw new Exception("Could not sent report.");
                }
                catch (Exception e)
                {
                    dispatch(new SetWrongPlaceReportCompleteAction(e));
                }
            };
        }
    }
}
