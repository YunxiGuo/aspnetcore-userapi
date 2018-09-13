using System.Threading.Tasks;

namespace User.Identity.Service
{
    public class TestAuthCodeService : IAuthCodeService
    {
        public async Task<bool> CheckAuthCodeAsync(string phone, string authcode)
        {
            return true;
        }
    }
}