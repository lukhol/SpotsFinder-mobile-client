using SpotFinder.Redux.StateModels;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IUserService
    {
        Task<User> RegisterUsingFacebookAsyc(User userToRegister, string accessToken);
        Task<User> RegisterAsync(User userToRegister);
        Task<User> LoginAsync(string email, string password);
    }
}
