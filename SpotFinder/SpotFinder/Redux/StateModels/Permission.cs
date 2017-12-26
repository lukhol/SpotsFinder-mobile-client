
using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.StateModels
{
    public class Permission
    {
        public PermissionStatus PermissionStatus { get; private set; }   
        public bool IsGetting { get; private set; }

        public Permission()
        {
            PermissionStatus = PermissionStatus.Unknown;
            IsGetting = false;
        }

        public Permission(PermissionStatus permissionStatus, bool isGetting)
        {
            PermissionStatus = permissionStatus;
            IsGetting = isGetting;
        }
    }
}
