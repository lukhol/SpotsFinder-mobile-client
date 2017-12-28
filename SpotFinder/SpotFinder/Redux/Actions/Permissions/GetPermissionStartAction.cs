using Redux;
using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.Actions.Permissions
{
    public class GetPermissionStartAction : IAction
    {
        public PermissionName PermissionName { get; private set; }

        public GetPermissionStartAction(PermissionName permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
