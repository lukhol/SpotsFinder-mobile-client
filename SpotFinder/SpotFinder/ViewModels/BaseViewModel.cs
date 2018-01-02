using Redux;
using SpotFinder.Redux;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected IList<IDisposable> subscriptions  = new List<IDisposable>();
        protected readonly IStore<ApplicationState> appStore;
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel()
        {

        }

        public BaseViewModel(IStore<ApplicationState> appStore)
        {
            this.appStore = appStore ?? throw new ArgumentNullException(nameof(appStore));
        }

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
            set => OnPropertyChanged();
        }

        public virtual ICommand GoBackCommand => new Command(() => 
        {
            App.Current.MainPage.Navigation.PopAsync();
        });

        public virtual ICommand PopToRootAsync => new Command(() =>
        {
            App.Current.MainPage.Navigation.PopToRootAsync();
        });

        public void CancelSubscriptions()
        {
            foreach (var sub in subscriptions)
                sub.Dispose();

            subscriptions.Clear();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
