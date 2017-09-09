using SpotFinder.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public class LightPlace
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PlaceType Type { get; set; }
    }
}
