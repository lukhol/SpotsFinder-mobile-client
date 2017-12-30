using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.Repositories;
using System;

namespace SpotFinder.Redux.Actions.CurrentPlace
{
    public class GetPlaceByIdActionCreator : IGetPlaceByIdActionCreator
    {
        private IPlaceService PlaceService { get; }
        private IPlaceRepository PlaceRepository { get; }

        public GetPlaceByIdActionCreator(IPlaceService placeService, IPlaceRepository placeRepository)
        {
            PlaceService = placeService ?? throw new ArgumentNullException(nameof(placeService));
            PlaceRepository = placeRepository ?? throw new ArgumentNullException(nameof(placeRepository));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> GetPlaceById(int id)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new GetPlaceByIdStartAction(id));

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
                       
                    dispatch(new GetPlaceByIdCompleteAction(place));
                }
                catch (Exception ex)
                {
                    //TODO: Log...
                    dispatch(new GetPlaceByIdCompleteAction(ex));
                }
            };
        }
    }
}
