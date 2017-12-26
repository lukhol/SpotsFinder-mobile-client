using SpotFinder.DataServices;
using System;

namespace SpotFinder.Redux.Actions.CurrentPlace
{
    public class DownloadPlaceByIdActionCreator : IDownloadPlaceByIdActionCreator
    {
        private IPlaceService PlaceService { get; }

        public DownloadPlaceByIdActionCreator(IPlaceService placeService)
        {
            PlaceService = placeService ?? throw new ArgumentNullException(nameof(placeService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlaceById(int id)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new DownloadPlaceByIdStartAction(id));

                var downloadedPace = await PlaceService.GetPlaceByIdAsync(id);

                dispatch(new DownloadPlaceByIdCompleteAction(downloadedPace));
            };
        }
    }
}
