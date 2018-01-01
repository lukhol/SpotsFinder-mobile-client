using SpotFinder.Models.DTO;
using System.Threading.Tasks;

namespace SpotFinder.DataServices
{
    public interface IErrorService
    {
        Task SendErrorInformationAsync(ErrorInfo errorInfo);
    }
}
