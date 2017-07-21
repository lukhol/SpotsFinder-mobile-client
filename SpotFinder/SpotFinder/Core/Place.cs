﻿using SpotFinder.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.Core
{
    public class Place
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public List<string> Photos;

        public bool Gap { get; set; }
        public bool Stairs { get; set; }
        public bool Rail { get; set; }
        public bool Ledge { get; set; }
        public bool Handrail { get; set; }
        public bool Corners { get; set; }
        public bool Manualpad { get; set; }
        public bool Wallride { get; set; }
        public bool Downhill { get; set; }
        public bool OpenYourMind { get; set; }
        public bool Pyramid { get; set; }
        public bool Curb { get; set; }
        public bool Bank { get; set; }
        public bool Bowl { get; set; }
        
        public ImageSource MainPhoto
        {
            get
            {
                var base64Image = Photos.ElementAt(0);
                var imageBytes = Convert.FromBase64String(base64Image);
                return ImageSource.FromStream(() => { return new MemoryStream(imageBytes); });
            }
        }

        public Place()
        {
            Gap = false;
            Stairs = false;
            Rail = false;
            Ledge = false;
            Handrail = false;
            Corners = false;
            Manualpad = false;
            Wallride = false;
            Downhill = false;
            OpenYourMind = false;
            Pyramid = false;
            Curb = false;
            Bank = false;
            Bowl = false;
        }

        public Command PlaceCommand => new Command(() =>
        {
            Application.Current.MainPage.Navigation.PushAsync(new PlaceDetailsPage(this));
        });
    }

    public enum Type
    {
        Skatepark, Skatespot, DIY
    }
}
