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
        protected readonly INavigationService NavigationService;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isBussy;
        public bool IsBussy
        {
            get => isBussy;
            set
            {
                isBussy = value;
                OnPropertyChanged();
            }
        }

        public BaseViewModel()
        {
            NavigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
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
