using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public class ReportManager
    {
        private Place place;

        public Place Place
        {
            get => place;
            set { place = value; }
        }

        public void CreateNewReport()
        {
            place = new Place();
        }
    }
}
