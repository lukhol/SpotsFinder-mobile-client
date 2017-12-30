using SpotFinder.Core.Enums;
using SpotFinder.PluginsExtensions;
using System;
using System.Threading.Tasks;
using PluginPermission = Plugin.Permissions;

namespace SpotFinder.Services
{
    public class CrossPermissionWrapper : IPermissions
    {
        PluginPermission.Abstractions.IPermissions currentPermissions;

        public CrossPermissionWrapper(PluginPermission.Abstractions.IPermissions currentPermissions)
        {
            this.currentPermissions = currentPermissions ??
                throw new ArgumentNullException(nameof(PluginPermission.Abstractions.IPermissions));
        }

        public async Task<PermissionStatus> CheckPermissionStatusAsync(PermissionName permissionName)
        {
            var pluginPermissionStauts = await currentPermissions.CheckPermissionStatusAsync(permissionName.ToPluginPermission());
            return pluginPermissionStauts.ToMyPermissionStatus();
        }

        public async Task RequestPermissionsAsync(PermissionName permissionName)
        {
            var result = await currentPermissions.RequestPermissionsAsync(permissionName.ToPluginPermission());
            //TODO: I should return result, and check it
        }
    }
}
