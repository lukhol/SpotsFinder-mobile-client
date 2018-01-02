using Redux;
using SpotFinder.Redux;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class ReportPlaceViewModel : BaseViewModel
    {
        public ReportPlaceViewModel(IStore<ApplicationState> appStore) : base(appStore)
        {

        }

        private ICommand SendReportCommand => new Command(SendReport);

        private string reasonComment;
        public string ReasonComment
        {
            get => reasonComment;
            set
            {
                reasonComment = value;
                OnPropertyChanged();
            }
        }

        private void SendReport()
        {

        }
    }
}
