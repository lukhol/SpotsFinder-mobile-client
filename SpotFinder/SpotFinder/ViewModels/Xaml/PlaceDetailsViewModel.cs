using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using SpotFinder.Models.Core;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels.Xaml
{
    public class PlaceDetailsViewModel : BaseViewModel
    {
        private Place place;

        public Place Place
        {
            get => place;
            set
            {
                place = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Description");
                OnPropertyChanged("ObstacleList");
                OnPropertyChanged("ObstacleListHeight");
                IsBusy = false;
            }
        }

        public PlaceDetailsViewModel(Place place)
        {
            this.place = place;

            if (place == null)
                IsBusy = true;
        }

        public PlaceDetailsViewModel()
        {
            IsBusy = true;
        }

        public string Name
        {
            get
            {
                if (place != null)
                    return place.Name;
                else
                    return "Place = null.";
            }
        }

        public string Description
        {
            get
            {
                if (place != null)
                    return place.Description;
                else
                    return "Place = null.";
            }
        }

        public List<string> ObstacleList
        {
            get
            {
                return PrepareObstacleList();
            }
        }

        public double ObstacleListHeight
        {
            get => PrepareObstacleList().Count * 20;
        }

        public ICommand GoBackCommand => new Command(() =>
        {
            App.Current.MainPage.Navigation.PopAsync();
        });

        private List<string> PrepareObstacleList()
        {
            var list = new List<string>();

            if (place == null)
            {
                list.Add("Place is null. Pleace report this bug.");
                return list;
            }

            if (place.Stairs == true)
                list.Add("-Stairs");

            if (place.Rail == true)
                list.Add("-Rail");

            if (place.Ledge == true)
                list.Add("-Ledge");

            if (place.Handrail == true)
                list.Add("-Handrail");

            if (place.Hubba == true)
                list.Add("-Hubba");

            if (place.Corners == true)
                list.Add("-Corners");

            if (place.Manualpad == true)
                list.Add("-Manualpad");

            if (place.Wallride == true)
                list.Add("-Wallride");

            if (place.Downhill == true)
                list.Add("-Downhill");

            if (place.OpenYourMind == true)
                list.Add("-OpenYourMind");

            if (place.Pyramid == true)
                list.Add("-Pyramid");

            if (place.Curb == true)
                list.Add("-Curb");

            if (place.Bank == true)
                list.Add("-Bank");

            if (place.Bowl == true)
                list.Add("-Bowl");

            return list;
        }

        public void Update()
        {
            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();
            Place = reportManager.ShowingPlace;
        }
    }
}
