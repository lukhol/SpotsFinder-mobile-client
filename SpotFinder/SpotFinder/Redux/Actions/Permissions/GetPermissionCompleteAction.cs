using Redux;
using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.Actions.Permissions
{
    public class GetPermissionCompleteAction : IAction
    {
        public PermissionName PermissionName { get; private set; }
        public PermissionStatus PermissionStatus { get; private set; }

        public GetPermissionCompleteAction(PermissionName permissionName, PermissionStatus permissionStatus)
        {
            PermissionName = permissionName;
            PermissionStatus = permissionStatus;
        }
    }
}
