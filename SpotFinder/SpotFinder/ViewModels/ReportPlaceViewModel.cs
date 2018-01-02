using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
using SpotFinder.Redux.Actions.WrongPlaceReports;
using SpotFinder.Views;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class ReportPlaceViewModel : BaseViewModel
    {
        private readonly ISetWrongPlaceReportActionCreator setWrongPlaceReportActionCreator;

        public ReportPlaceViewModel(IStore<ApplicationState> appStore, 
            ISetWrongPlaceReportActionCreator setWrongPlaceReportActionCreator) : base(appStore)
        {
            this.setWrongPlaceReportActionCreator = setWrongPlaceReportActionCreator ?? throw new ArgumentNullException(nameof(setWrongPlaceReportActionCreator));

            var sendWrongPlaceReportResultSub = appStore
                .DistinctUntilChanged(state => new { state.PlacesData.WrongPlaceReport.Status })
                .SubscribeWithError(state =>
                {
                    var wrongPlaceReportStatus = state.PlacesData.WrongPlaceReport.Status;
                    if(wrongPlaceReportStatus == Status.Success)
                    {
                        IsBusy = false;
                        App.Current.MainPage.DisplayAlert("Success!", "Thank you for your report.", "Ok");
                        if(App.Current.MainPage.Navigation.NavigationStack.Last().GetType() == typeof(ReportPlacePage))
                            App.Current.MainPage.Navigation.PopAsync();
                    }
                    else if(wrongPlaceReportStatus == Status.Error)
                    {
                        IsBusy = false;
                        App.Current.MainPage.DisplayAlert("Something went wrong.", "Report does not sent. Please try again.", "Ok");
                    }

                    appStore.Dispatch(new SetEmptyWrongPlaceReportAction());

                }, error => { appStore.Dispatch(new SetErrorAction(error, "ReportPlaceViewModel - subscription.")); });

            subscriptions.Add(sendWrongPlaceReportResultSub);
        }

        public  ICommand SendReportCommand => new Command(SendReport);

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

        private bool isNotSkateboardPlace;
        public bool IsNotSkateboardPlace
        {
            get => isNotSkateboardPlace;
            set
            {
                isNotSkateboardPlace = value;
                OnPropertyChanged();
            }
        }

        private void SendReport()
        {
            IsBusy = true;
            appStore.DispatchAsync(setWrongPlaceReportActionCreator.UploadWrongPlaceReport(reasonComment, isNotSkateboardPlace));
        }
    }
}
