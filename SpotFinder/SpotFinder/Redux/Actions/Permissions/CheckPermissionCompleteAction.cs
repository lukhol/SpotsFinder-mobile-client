using Redux;
using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.Actions.Permissions
{
    public class CheckPermissionCompleteAction : IAction
    {
        public PermissionName PermissionName { get; private set; }
        public PermissionStatus PermissionStatus { get; private set; }

        public CheckPermissionCompleteAction(PermissionName permissionName, PermissionStatus permissionStatus)
        {
            PermissionName = permissionName;
            PermissionStatus = permissionStatus;
        }
    }
}
