using SpotFinder.Models.Core;
using System.Collections.Generic;

namespace SpotFinder.Repositories
{
    public interface IPlaceRepository
    {
        Place GetPlace(int id);
        bool InsertPlace(Place place);
        bool ExistPlace(int id);
    }
}
