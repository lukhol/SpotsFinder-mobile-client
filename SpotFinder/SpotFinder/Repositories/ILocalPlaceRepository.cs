using SpotFinder.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Repositories
{
    public interface ILocalPlaceRepository
    {
        bool InsertPlace(Place place);
        Place GetPlace(int id);
        List<Place> GetAllPlaces();

        Place GetPlaceOryginal(int id);
        bool InsertPlaceOryginal(Place place);
    }
}
