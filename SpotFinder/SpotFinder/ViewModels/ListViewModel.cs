using SpotFinder.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class ListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private INavigation Navigation { get; }
        private IPlaceRepository PlaceRepository { get; }

        public List<Place> PlaceList { get; set; }

        public ListViewModel(INavigation navigation, IPlaceRepository placeRepository)
        {
            PlaceRepository = placeRepository;
            Navigation = navigation;
            PlaceList = placeRepository.GetAllPlace();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
