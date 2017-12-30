using SpotFinder.Core.Enums;
using System.Threading.Tasks;

namespace SpotFinder.Services
{
    public interface IPermissions
    {
        Task<PermissionStatus> CheckPermissionStatusAsync(PermissionName permissionName);
        Task RequestPermissionsAsync(PermissionName permissionName);
    }
}
