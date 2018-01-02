﻿using Redux;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.CurrentPlace;
using SpotFinder.Resx;
using SpotFinder.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class ListViewModel : BaseViewModel
    {
        private IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator;

        public ListViewModel(IStore<ApplicationState> appStore, IGetPlaceByIdActionCreator downloadPlaceByIdActionCreator) : base(appStore)
        {
            this.downloadPlaceByIdActionCreator = downloadPlaceByIdActionCreator ?? throw new ArgumentNullException(nameof(downloadPlaceByIdActionCreator));

            appStore
                .DistinctUntilChanged(state => new { state.PlacesData.PlacesListState.Status })
                .Subscribe(state =>
                {
                    var placesList = state.PlacesData.PlacesListState.Value;
                    if (placesList != null && state.PlacesData.PlacesListState.Status == Core.Enums.Status.Success)
                    {
                        UpdateList(placesList);
                    }
                    else if (state.PlacesData.PlacesListState.Status == Core.Enums.Status.Error)
                    {
                        IsBusy = false;
                        IsPromptVisible = true;
                    }
                    else
                    {
                        IsBusy = true;
                    }
                }, error => { appStore.Dispatch(new SetErrorAction(error, "ListViewModel in subscription.")); });

            InformationText = "TODO: Add error text to resources.";
        }

        private void UpdateList(IList<Place> places)
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
                if (place.PhotosBase64List.Count > 0)
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
                appStore.DispatchAsync(downloadPlaceByIdActionCreator.GetPlaceById(selectedPlace.Id, selectedPlace.Version));
                App.Current.MainPage.Navigation.PushAsync(new PlaceDetailsPage());
            }
        });
    }
}
