using SpotFinder.Models.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IPlaceService
    {
        Task<List<Place>> GetAllPlaceAsync();
        Task<List<Place>> GetPlacesByCriteriaAsync(Criteria criteria);
        Task<int> SendAsync(Place place);
        Task<Place> GetPlaceById(int id);
    }
}
