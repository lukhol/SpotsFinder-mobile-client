using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class DownloadPlacesListByCriteriaActionCreator : IDownloadPlacesListByCriteriaActionCreator
    {
        private IPlaceService PlaceService { get; }

        public DownloadPlacesListByCriteriaActionCreator(IPlaceService placeService)
        {
            PlaceService = placeService ?? throw new ArgumentNullException(nameof(placeService));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> DownloadPlaceByCriteria(Criteria criteria)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new DownloadPlacesListByCriteriaStartAction(criteria));

                var placesList = await PlaceService.GetPlacesByCriteriaAsync(criteria);

                dispatch(new DownloadPlacesListByCriteriaCompleteAction(placesList));
            };;
        }
    }
}
