using Plugin.Permissions.Abstractions;
using MyEnums = SpotFinder.Core.Enums;

namespace SpotFinder.PluginsExtensions
{
    public static class CrossPermissionExtension
    {
        public static MyEnums.PermissionName ToMyPermissionName(this Permission permission)
        {
            switch (permission)
            {
                case Permission.Camera:
                    return MyEnums.PermissionName.Camera;
                case Permission.Location:
                    return MyEnums.PermissionName.Location;
                case Permission.Storage:
                    return MyEnums.PermissionName.Storage;
                default:
                    return MyEnums.PermissionName.Unkown;
            }
        }

        public static Permission ToPluginPermission(this MyEnums.PermissionName permissionName)
        {
            switch (permissionName)
            {
                case MyEnums.PermissionName.Camera:
                    return Permission.Camera;
                case MyEnums.PermissionName.Location:
                    return Permission.Location;
                case MyEnums.PermissionName.Storage:
                    return Permission.Storage;
                default:
                    return Permission.Unknown;
            }
        }

        public static MyEnums.PermissionStatus ToMyPermissionStatus(this PermissionStatus permissionStatus)
        {
            switch (permissionStatus)
            {
                case PermissionStatus.Denied:
                    return MyEnums.PermissionStatus.Denied;
                case PermissionStatus.Disabled:
                    return MyEnums.PermissionStatus.Disabled;
                case PermissionStatus.Granted:
                    return MyEnums.PermissionStatus.Granted;
                case PermissionStatus.Restricted:
                    return MyEnums.PermissionStatus.Restricted;
                default:
                    return MyEnums.PermissionStatus.Unknown;
            }
        }

        public static PermissionStatus ToPluginPermissionStatus(this MyEnums.PermissionStatus myPermissionStatus)
        {
            switch (myPermissionStatus)
            {
                case MyEnums.PermissionStatus.Denied:
                    return PermissionStatus.Denied;
                case MyEnums.PermissionStatus.Disabled:
                    return PermissionStatus.Disabled;
                case MyEnums.PermissionStatus.Granted:
                    return PermissionStatus.Granted;
                case MyEnums.PermissionStatus.Restricted:
                    return PermissionStatus.Restricted;
                default:
                    return PermissionStatus.Unknown;
            }
        }
    }
}
