namespace SpotFinder.Repositories
{
    public interface IURLRepository
    {
        string PostErrorUri { get; }
        string GetPlacesUri { get; }
        string PostPlaceUri { get; }
        string GetPlaceByCriteriaUri { get; }
        string GetPlaceByIdUri(int id);
        string PostWrongPlaceReportUri { get; }
        string LoginUri(string email, string password);
        string TokensUri();
        string GetFacebookUserInfoUri(string accessToken);
        string PostExternalUserUri(string accessToken);
        string GetGoogleUserInfoUri(string accessToken);
        string RegisterUserUri();

        string API_KEY { get; }
    }
}
