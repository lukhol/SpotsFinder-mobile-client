﻿using Newtonsoft.Json;
using SpotFinder.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace SpotFinder.Models.Core
{
    public class Place
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PlaceType Type { get; set; }
        public IList<string> PhotosBase64;

        [JsonIgnore]
        public IList<Image> PhotosAsImage;

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
        public bool Hubba { get; set; }
        
        [JsonIgnore]
        public ImageSource MainPhoto
        {
            get
            {
                var base64Image = PhotosBase64.ElementAt(0);
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
            Description = string.Empty;
            Name = string.Empty;
            Location = new Location();
            PhotosBase64 = new List<string>();
            PhotosAsImage = new List<Image>();
        }
    }
}
