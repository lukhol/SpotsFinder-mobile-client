using SpotFinder.Core.Enums;
using SpotFinder.Services;
using System;
using System.Threading.Tasks;

namespace SpotFinder.Redux.Actions.Permissions
{
    public class PermissionActionCreator : IPermissionActionCreator
    {
        private IPermissions permissions;

        public PermissionActionCreator(IPermissions permissions)
        {
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        }

        public StoreExtensions.AsyncActionCreator<ApplicationState> CheckPermissions(params PermissionName[] permissionsNames)
        {
            return async (dispatch, getState) =>
            {
                foreach(PermissionName permissionName in permissionsNames)
                {
                    dispatch(new GetPermissionStartAction(permissionName));

                    var result = await CheckPermissionAsync(permissionName);
                    if (result != PermissionStatus.Granted)
                    {
                        await permissions.RequestPermissionsAsync(permissionName);
                        result = await CheckPermissionAsync(permissionName);
                    }

                    dispatch(new GetPermissionCompleteAction(permissionName, result));
                }
            };
        }

        private async Task<PermissionStatus> CheckPermissionAsync(PermissionName permissionNameToCheck)
        {
            var crossPermissionsStatus = await permissions.CheckPermissionStatusAsync(permissionNameToCheck);
            return crossPermissionsStatus;
        }
    }
}
