using SpotFinder.Models.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IPlaceService
    {
        Task<IList<Place>> GetAllAsync();
        Task<IList<Place>> GetByCriteriaAsync(Criteria criteria);
        Task<Place> GetByIdAsync(int id);
        Task<int> SendAsync(Place place);
        Task<Place> UpdateAsync(Place place, long placeId);
        Task<IList<Place>> GetByUserIdAsync(long userId);
    }
}
