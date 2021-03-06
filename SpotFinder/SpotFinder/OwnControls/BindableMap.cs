﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.OwnControls
{
    public class BindableMap : Map
    {
        public static readonly BindableProperty MapSpanProperty
            = BindableProperty.Create(nameof(MapSpan),
                                      typeof(MapSpan),
                                      typeof(BindableMap),
                                      null,
                                      propertyChanged: (bindable, old, newValue) =>
                                      {
                                          var bindableMap = bindable as BindableMap;
                                          var newMapSpan = newValue as MapSpan;

                                          bindableMap.MoveToRegion(newMapSpan);
                                      });

        public MapSpan MapSpan;

        public static readonly BindableProperty MapPositionProperty
            = BindableProperty.Create(nameof(MapPosition),
                                      typeof(Position),
                                      typeof(BindableMap),
                                      new Position(0,0),
                                      propertyChanged: (b, o, n) => 
                                      {
                                          var bindableMap = b as BindableMap;
                                          if(bindableMap != null)
                                            bindableMap.MoveToRegion(MapSpan.FromCenterAndRadius((Position)n, Distance.FromMeters(50)));
                                      });

        public Position MapPosition { get; set; }

        public static readonly BindableProperty MapPinsProperty = BindableProperty.Create(
             nameof(Pins),
             typeof(ObservableCollection<Pin>),
             typeof(BindableMap),
             new ObservableCollection<Pin>(),
             defaultBindingMode: BindingMode.TwoWay,
             propertyChanged: (b, o, n) =>
             {
                 var bindable = (BindableMap)b;
                 bindable.Pins.Clear();

                 var collection = (ObservableCollection<Pin>)n;

                 if (collection == null)
                     return;

                 foreach (var item in collection)
                     bindable.Pins.Add(item);

                 collection.CollectionChanged += (sender, e) =>
                 {
                     Device.BeginInvokeOnMainThread(() =>
                     {
                         switch (e.Action)
                         {
                             case NotifyCollectionChangedAction.Add:
                             case NotifyCollectionChangedAction.Replace:
                             case NotifyCollectionChangedAction.Remove:
                                 if (e.OldItems != null)
                                     foreach (var item in e.OldItems)
                                         bindable.Pins.Remove((Pin)item);
                                 if (e.NewItems != null)
                                     foreach (var item in e.NewItems)
                                         bindable.Pins.Add((Pin)item);
                                 break;
                             case NotifyCollectionChangedAction.Reset:
                                 bindable.Pins.Clear();
                                 break;
                         }
                     });
                 };
             });

        public ObservableCollection<Pin> MapPins { get; set; }
    }
}
