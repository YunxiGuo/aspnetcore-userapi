using System.Threading.Tasks;

namespace User.Identity.Service
{
    public interface IAuthCodeService
    {
        Task<bool> CheckAuthCodeAsync(string authcode);
    }
}