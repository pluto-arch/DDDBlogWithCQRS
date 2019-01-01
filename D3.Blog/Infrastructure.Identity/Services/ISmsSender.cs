using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}