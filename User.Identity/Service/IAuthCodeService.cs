using System.Threading.Tasks;

namespace User.Identity.Service
{
    public interface IAuthCodeService
    {
        /// <summary>
        /// 根据手机号来验证验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="authcode">验证码</param>
        /// <returns></returns>
        Task<bool> CheckAuthCodeAsync(string phone,string authcode);
    }
}