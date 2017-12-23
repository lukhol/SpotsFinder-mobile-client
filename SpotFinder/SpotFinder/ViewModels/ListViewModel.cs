using SpotFinder.Models.Core;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using SpotFinder.Resx;
using SpotFinder.Views;
using SpotFinder.Redux.Actions;
using System.Linq;

namespace SpotFinder.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel()
        {
            App.AppStore
                .DistinctUntilChanged(state => new { state.PlacesData.ListOfPlaces })
                .Subscribe(state =>
                {
                    if(state.PlacesData.ListOfPlaces == null)
                    {
                        IsBusy = true;
                        IsPromptVisible = false;
                    }
                    else
                    {
                        UpdateList(state.PlacesData.ListOfPlaces);
                        IsBusy = false;
                    }
                });
        }

        private void UpdateList(List<Place> places)
        {
            if (places == null || places.Count == 0)
            {
                observablePlaceList.Clear();
                informationText = AppResources.CountSpotsInformationNotFound;
                IsBusy = false;
                IsPromptVisible = true;

                OnPropertyChanged("ObservablePlaceList");
                OnPropertyChanged("InformationText");

                return;
            }

            observablePlaceList.Clear();
            foreach(var place in places)
            {
                if (place.PhotosBase64.Count > 0)
                    observablePlaceList.Add(place);
            }


            IsBusy = false;
            IsPromptVisible = false;

            OnPropertyChanged("ObservablePlaceList");
            OnPropertyChanged("InformationText");
        }

        private string informationText;
        public string InformationText
        {
            get => informationText;
            set
            {
                informationText = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Place> observablePlaceList = new ObservableCollection<Place>();
        public ObservableCollection<Place> ObservablePlaceList
        {
            get => observablePlaceList;
            set
            {
                observablePlaceList = value;
                OnPropertyChanged();
            }
        }

        private bool isPromptVisible;
        public bool IsPromptVisible
        {
            get => isPromptVisible;
            set
            {
                isPromptVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnListViewItemSelectedCommand => new Command((param) =>
        {
            var selectedPlace = param as Place;

            if (selectedPlace == null)
                return;

            if(App.Current.MainPage.Navigation.NavigationStack.Last().GetType() == typeof(ListPage))
            {
                App.AppStore.Dispatch(new ClearActuallyShowingPlaceAction());
                App.AppStore.Dispatch(new RequestDownloadSpotAction(selectedPlace.Id));

                App.Current.MainPage.Navigation.PushAsync(new PlaceDetailsPage());
            }
        });
    }
}
