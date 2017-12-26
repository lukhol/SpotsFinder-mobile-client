using SpotFinder.Core.Enums;

namespace SpotFinder.Redux.ActionsCreators
{
    public interface IPermissionActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> CheckPermissions(params PermissionName[] permissionsNames);
    }
}
