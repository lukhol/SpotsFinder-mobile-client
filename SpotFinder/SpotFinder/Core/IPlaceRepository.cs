﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public interface IPlaceRepository
    {
        List<Place> GetAllPlace();
    }
}
