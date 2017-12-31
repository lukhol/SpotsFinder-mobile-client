namespace SpotFinder.Repositories
{
    public interface IURLRepository
    {
        string PostErrorUri { get; }
        string GetPlacesUri { get; }
        string PostPlaceUri { get; }
        string GetPlaceByCriteriaUri { get; }
        string GetPlaceByIdUri(int id);

        string API_KEY { get; }
    }
}
