using BuilderImmutableObject;
using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.StateModels;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;

namespace SpotFinder.Redux.Reducers
{
    public class PermissionsReducer : IReducer<IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>>>
    {
        public IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>>
            Reduce(IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>> previousState, IAction action)
        {
            if(action is CheckPermissionStartAction)
            {
                var checkPermissionStartAction = action as CheckPermissionStartAction;
                var permissionName = checkPermissionStartAction.PermissionName;

                return previousState
                    .Select(x =>
                    {
                        if (x.Key == permissionName)
                            return new KeyValuePair<PermissionName, AsyncOperationState<PermissionStatus, Unit>>
                                (x.Key, x.Value.Set(v => v.Status, Status.Getting).Build());
                        else
                            return x;
                    })
                    .ToImmutableDictionary();
            }

            if(action is CheckPermissionCompleteAction)
            {
                var checkPermissionCompleteAction = action as CheckPermissionCompleteAction;

                return previousState
                    .Select(x =>
                    {
                        if (x.Key == checkPermissionCompleteAction.PermissionName)
                            return new KeyValuePair<PermissionName, AsyncOperationState<PermissionStatus, Unit>>(x.Key,
                                x.Value.Set(v => v.Value, checkPermissionCompleteAction.PermissionStatus)
                                       .Set(v => v.Status, Status.Success)
                                       .Build());
                        else
                            return x;
                    })
                    .ToImmutableDictionary();
            }

            return previousState;
        }
    }
}