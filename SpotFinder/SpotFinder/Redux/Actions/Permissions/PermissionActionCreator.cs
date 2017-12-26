using Plugin.Permissions;
using SpotFinder.Core.Enums;
using SpotFinder.PluginsExtensions;
using System.Threading.Tasks;

namespace SpotFinder.Redux.Actions.Permissions
{
    public class PermissionActionCreator : IPermissionActionCreator
    {
        public StoreExtensions.AsyncActionCreator<ApplicationState> CheckPermissions(params PermissionName[] permissionsNames)
        {
            return async (dispatch, getState) =>
            {
                foreach(PermissionName permissionName in permissionsNames)
                {
                    dispatch(new CheckPermissionStartAction(permissionName));

                    var result = await CheckPermissionAsync(permissionName);
                    if (result != PermissionStatus.Granted)
                    {
                        await CrossPermissions.Current.RequestPermissionsAsync(permissionName.ToPluginPermission());
                        result = await CheckPermissionAsync(permissionName);
                    }

                    dispatch(new CheckPermissionCompleteAction(permissionName, result));
                }
            };
        }

        private async Task<PermissionStatus> CheckPermissionAsync(PermissionName permissionNameToCheck)
        {
            var permissionToCheck = permissionNameToCheck.ToPluginPermission();
            var crossPermissionsStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permissionToCheck);
            return crossPermissionsStatus.ToMyPermissionStatus();
            
        }
    }
}
