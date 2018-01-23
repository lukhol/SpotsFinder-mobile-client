using SpotFinder.Models.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IPlaceService
    {
        Task<List<Place>> GetAllAsync();
        Task<List<Place>> GetByCriteriaAsync(Criteria criteria);
        Task<Place> GetByIdAsync(int id);
        Task<int> SendAsync(Place place);
        Task<IList<Place>> GetByUserIdAsync(long userId);
    }
}
