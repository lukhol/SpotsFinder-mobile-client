using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public interface IPlaceRepository
    {
        Task<List<Place>> GetAllPlace();
        Task<List<Place>> GetPlacesByCriteria(Criteria criteria);
        Task<int> SendAsync(Place place);
    }
}
