﻿using System;
using System.Collections.Generic;
using SpotFinder.Redux.StateModels;
using System.Collections.Immutable;
using SpotFinder.Core.Enums;

namespace SpotFinder.Redux
{
    public class ApplicationState
    {
        //Permissions:
        public IImmutableDictionary<PermissionName, Permission> PermissionsDictionary { get; set; }

        //Navigation section
        public Stack<PageName> NavigationStack { get; set; }

        //Settings section
        public Settings Settings { get; set; }

        //Data section
        public PlacesData PlacesData { get; set; }

        //Device information section
        public DeviceData DeviceData { get; set; }

        public ApplicationState()
        {
            PlacesData = new PlacesData();
            DeviceData = new DeviceData();
            var permissionDictionary = new Dictionary<PermissionName, Permission>();
            permissionDictionary.Add(PermissionName.Camera, new Permission());
            permissionDictionary.Add(PermissionName.Location, new Permission());
            permissionDictionary.Add(PermissionName.Storage, new Permission());
            PermissionsDictionary = permissionDictionary.ToImmutableDictionary();
        }
    }
}
