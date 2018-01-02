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

        public StoreExtensions.AsyncActionCreator<ApplicationState> GetPlaceById(int id, long version)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new GetPlaceByIdStartAction(id));

                try
                {
                    Place place;
                    var isPlaceInLocalDatabase = PlaceRepository.ExistPlace(id);
                    //TODO: Refactor!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    if (isPlaceInLocalDatabase)
                    {
                        place = PlaceRepository.GetPlace(id);
                        if (version > place.Version)
                        {
                            place = await PlaceService.GetPlaceByIdAsync(id);
                            PlaceRepository.InsertPlace(place);
                        }
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
