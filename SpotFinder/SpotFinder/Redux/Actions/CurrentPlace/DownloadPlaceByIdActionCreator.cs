using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.Repositories;
using System;

namespace SpotFinder.Redux.Actions.CurrentPlace
{
    public class DownloadPlaceByIdActionCreator : IDownloadPlaceByIdActionCreator
    {
        private IPlaceService PlaceService { get; }
        private IPlaceRepository PlaceRepository { get; }

        public DownloadPlaceByIdActionCreator(IPlaceService placeService, IPlaceRepository placeRepository)
        {
            PlaceService = placeService ?? throw new ArgumentNullException(nameof(placeService));
            PlaceRepository = placeRepository ?? throw new ArgumentNullException(nameof(placeRepository));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlaceById(int id)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new DownloadPlaceByIdStartAction(id));

                try
                {
                    Place place;
                    var isPlaceInLocalDatabase = PlaceRepository.ExistPlace(id);

                    if (isPlaceInLocalDatabase)
                    {
                        place = PlaceRepository.GetPlace(id);
                    }
                    else
                    {
                        place = await PlaceService.GetPlaceByIdAsync(id);
                        PlaceRepository.InsertPlace(place);
                    }
                       
                    dispatch(new DownloadPlaceByIdCompleteAction(place));
                }
                catch (Exception ex)
                {
                    //TODO: Log...
                    dispatch(new DownloadPlaceByIdErrorCompleteAction(ex));
                }
            };
        }
    }
}
