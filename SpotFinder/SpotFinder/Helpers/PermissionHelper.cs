using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace SpotFinder.Helpers
{
    public class PermissionHelper : IPermissionHelper
    {
        public async void CheckAllPermissionAsync()
        {
            var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (locationStatus != PermissionStatus.Granted)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

            var cameraPermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraPermission != PermissionStatus.Granted)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var storagePermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storagePermission != PermissionStatus.Granted)
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
        }
    }
}
