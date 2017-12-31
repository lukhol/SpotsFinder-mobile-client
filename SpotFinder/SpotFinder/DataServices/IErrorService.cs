using SpotFinder.Models.DTO;

namespace SpotFinder.DataServices
{
    public interface IErrorService
    {
        void SendErrorInformation(ErrorInfo errorInfo);
    }
}
