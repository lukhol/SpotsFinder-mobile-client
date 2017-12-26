using Redux;
using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.Actions.Permissions
{
    public class CheckPermissionStartAction : IAction
    {
        public PermissionName PermissionName { get; private set; }

        public CheckPermissionStartAction(PermissionName permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
