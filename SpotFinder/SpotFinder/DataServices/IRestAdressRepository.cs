using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IRestAdressRepository
    {
        Task<Position> GetPositionOfTheCity(string cityName, bool sensor);
    }
}
