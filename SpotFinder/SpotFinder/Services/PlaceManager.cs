using SpotFinder.DataServices;
using SpotFinder.Models.Core;
using SpotFinder.Redux.Actions;
using SpotFinder.Repositories;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SpotFinder.Services
{
    public class PlaceManager : IPlaceManager
    {
        private IPlaceService PlaceService { get; }
        private ILocalPlaceRepository LocalPlaceRepository;

        public PlaceManager(IPlaceService placeService, ILocalPlaceRepository localPlaceRepository)
        {
            PlaceService = placeService ?? throw new ArgumentNullException("PlaceService is null in PlaceManager.");
            LocalPlaceRepository = localPlaceRepository ?? throw new ArgumentNullException("LocalPlaceRepository is null in PlaceManager");
        }

        public async void DownloadPlacesByCriteriaAsync(Criteria criteria)
        {
            List<Place> downloadedPlace = null;

            try
            {
                downloadedPlace = await PlaceService.GetPlacesByCriteriaAsync(criteria);
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage.DisplayAlert("Error.", "Cannot download spots because no internet connection or server error", "Ok");
                });
            }

            App.AppStore.Dispatch(new SetListOfPlacesAction(downloadedPlace));
        }

        //public async void DownloadSinglePlaceByIdAsync(int id)
        //{
        //    Place placeToShow = null;

        //    var placeFromLocalRepository = LocalPlaceRepository.GetPlaceOryginal(id);
        //    if(placeFromLocalRepository == null)
        //    {
        //        placeToShow = await PlaceService.GetPlaceByIdAsync(id);
        //    }
        //    else
        //    {
        //        placeToShow = placeFromLocalRepository;
        //    }

        //    if (placeToShow != null)
        //        LocalPlaceRepository.InsertPlaceOryginal(placeToShow);

        //    App.AppStore.Dispatch(new PassSpotToShowAction(placeToShow));
        //}

        public void UploadPlaceAsync(Place place)
        {
            throw new NotImplementedException();
        }
    }
}
