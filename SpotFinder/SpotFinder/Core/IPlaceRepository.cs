using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public interface IPlaceRepository
    {
        Task<List<Place>> GetAllPlaceAsync();
        Task<List<Place>> GetPlacesByCriteriaAsync(Criteria criteria);
        Task<int> SendAsync(Place place);
    }
}
