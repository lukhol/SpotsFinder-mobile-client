using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IExternalUserService<T>
    {
        Task<string> GetAccessTokenAsync(string code);
        Task<T> GetExternalUserInfoAsync(string accessToken);
    }
}
