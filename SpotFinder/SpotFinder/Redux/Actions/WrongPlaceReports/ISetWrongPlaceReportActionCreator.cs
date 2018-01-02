namespace SpotFinder.Redux.Actions.WrongPlaceReports
{
    public interface ISetWrongPlaceReportActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> 
            UploadWrongPlaceReport(string reasonComment, bool isNotSkateboardPlace);
    }
}
