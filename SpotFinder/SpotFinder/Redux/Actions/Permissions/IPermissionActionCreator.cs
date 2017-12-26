using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.Actions.Permissions
{
    public interface IPermissionActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> CheckPermissions(params PermissionName[] permissionsNames);
    }
}
