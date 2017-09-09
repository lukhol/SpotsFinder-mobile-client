using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public interface ILocalPlaceRepository
    {
        Task<bool> InsertPlaceAsync(Place place);
        Place GetPlace(int id);
        Task<List<Place>> GetAllPlacesAsync();

        Place GetPlaceOryginal(int id);
    }
}
