﻿using SpotFinder.Models.DTO;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IFacebookService
    {
        Task<SimpleFacebookUserDTO> GetSimpleFacebookInfoAsync(string accessToken);
    }
}
