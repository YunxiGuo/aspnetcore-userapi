using System.Collections.Generic;

namespace User.API.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avater { get; set; }

        /// <summary>
        /// 性别,1 -- 男,0 -- 女
        /// </summary>
        public byte Gender { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        public string Province { get; set; }

        public int CityId { get; set; }

        public string City { get; set; }

        public string NameCard { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        public List<UserProperty> UserProperties { get; set; }
    }
}