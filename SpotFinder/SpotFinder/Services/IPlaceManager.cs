using SpotFinder.Models.Core;

namespace SpotFinder.Services
{
    public interface IPlaceManager
    {
        void DownloadPlacesByCriteriaAsync(Criteria criteria);
        void UploadPlaceAsync(Place place);
    }
}
