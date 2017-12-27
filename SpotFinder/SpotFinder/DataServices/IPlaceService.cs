using SpotFinder.Models.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IPlaceService
    {
        Task<List<Place>> GetAllPlacesAsync();
        Task<List<Place>> GetPlacesByCriteriaAsync(Criteria criteria);
        Task<int> SendAsync(Place place);
        Task<Place> GetPlaceByIdAsync(int id);
    }
}
