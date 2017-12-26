using BuilderImmutableObject;
using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Redux.Actions.Permissions;
using SpotFinder.Redux.StateModels;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SpotFinder.Redux.Reducers
{
    public class PermissionsReducer : IReducer<IImmutableDictionary<PermissionName, Permission>>
    {
        public IImmutableDictionary<PermissionName, Permission> Reduce(IImmutableDictionary<PermissionName, Permission> previousState, IAction action)
        {
            if(action is CheckPermissionStartAction)
            {
                var checkPermissionStartAction = action as CheckPermissionStartAction;
                var permissionName = checkPermissionStartAction.PermissionName;

                return previousState
                    .Select(x =>
                    {
                        if (x.Key == permissionName)
                            return new KeyValuePair<PermissionName, Permission>(x.Key, x.Value.Set(v => v.IsGetting, true).Build());
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
                            return new KeyValuePair<PermissionName, Permission>(x.Key,
                                x.Value.Set(v => v.PermissionStatus, checkPermissionCompleteAction.PermissionStatus)
                                       .Set(v => v.IsGetting, false)
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


//foreach(var permissionDictionaryItem in previousState)
//{
//    if (permissionDictionaryItem.Key.Equals(permissionName))
//        newState.Add(permissionDictionaryItem.Key, permissionDictionaryItem.Value
//                                                                           .Set(v => v.IsGetting, true)
//                                                                           .Build());
//    else
//        newState.Add(permissionDictionaryItem.Key, permissionDictionaryItem.Value);

//}

//return newState.ToImmutableDictionary();

