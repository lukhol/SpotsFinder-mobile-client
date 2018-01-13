using SpotFinder.Core.Enums;
using SpotFinder.OwnControls;
using System.IO;
using System.Threading.Tasks;

namespace SpotFinder.Services
{
    public interface IPhotoProvider
    {
        Task<MyImage> GetPhotoAsync(GetPhotoType photoType);
        Task<Stream> GetPhotoAsStreamAsync(GetPhotoType getPhotoType);
    }
}
