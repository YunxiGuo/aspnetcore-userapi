using System.Threading.Tasks;

namespace User.Identity.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>返回用户的Id</returns>
        Task ValidationAsync(string phone);
    }
}