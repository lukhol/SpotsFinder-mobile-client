using SpotFinder.Services;
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
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly NavigationService NavigationService;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                IsMainContentVisible = !value;
                OnPropertyChanged();
            }
        }

        public bool IsMainContentVisible
        {
            get => !IsBusy;
            set
            {
                OnPropertyChanged();
            }
        }

        public BaseViewModel()
        {
            //NavigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
        }

        public virtual void InjectPage(Page page)
        {

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
