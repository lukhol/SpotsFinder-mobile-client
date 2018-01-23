using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class GetPlacesListActionCreator : IGetPlacesListActionCreator
    {
        private IPlaceService PlaceService { get; }

        public GetPlacesListActionCreator(IPlaceService placeService)
        {
            PlaceService = placeService ?? throw new ArgumentNullException(nameof(placeService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlacesByCriteria(Criteria criteria)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new GetPlacesStartAction<Criteria>(criteria));

                try
                {
                    var placesList = await PlaceService.GetByCriteriaAsync(criteria);
                    dispatch(new GetPlacesListByCriteriaCompleteAction(placesList));
                }
                catch (Exception e)
                {
                    dispatch(new GetPlacesListByCriteriaCompleteAction(e));
                }
            };
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlacesByUserId(long userId)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new GetPlacesStartAction<long>(userId));

                try
                {
                    var placesList = await PlaceService.GetByUserIdAsync(userId);
                    dispatch(new GetPlacesListByUserIdCompleteAction(placesList));
                }
                catch (Exception e)
                {
                    dispatch(new GetPlacesListByUserIdCompleteAction(e));
                }
            };
        }
    }
}
