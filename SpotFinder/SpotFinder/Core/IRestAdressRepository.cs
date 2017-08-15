using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public interface IRestAdressRepository
    {
        Task<Position> GetPositionOfTheCity(string cityName, bool sensor);
    }
}
