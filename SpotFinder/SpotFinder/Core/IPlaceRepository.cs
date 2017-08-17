using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public interface IPlaceRepository
    {
        List<Place> GetAllPlace();
        Task<List<Place>> GetPlacesByCriteria(Criteria criteria);
        bool Send(Place place);
    }
}
