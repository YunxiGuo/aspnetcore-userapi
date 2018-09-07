using System;

namespace User.API.Models
{
    public class BPFile
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 上传的源文件地址
        /// </summary>
        public string OriginFilePath { get; set; }

        /// <summary>
        /// 格式转化后的文件地址
        /// </summary>
        public string FormatFilePath { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}