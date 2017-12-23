using SpotFinder.Models.Core;

namespace SpotFinder.Services
{
    public interface IPlaceManager
    {
        void DownloadSinglePlaceByIdAsync(int id);
        void DownloadPlacesByCriteriaAsync(Criteria criteria);
        void UploadPlaceAsync(Place place);
    }
}
