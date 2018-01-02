using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class GetPlacesListByCriteriaActionCreator : IGetPlacesListByCriteriaActionCreator
    {
        private IPlaceService PlaceService { get; }

        public GetPlacesListByCriteriaActionCreator(IPlaceService placeService)
        {
            PlaceService = placeService ?? throw new ArgumentNullException(nameof(placeService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlaceByCriteria(Criteria criteria)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new GetPlacesListByCriteriaStartAction(criteria));

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
    }
}
