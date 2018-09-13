using System.Threading.Tasks;

namespace User.Identity.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 检测手机号是否存在，若不存在则创建用户
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>返回用户的Id</returns>
        Task<int> CheckOrCreate(string phone);
    }
}