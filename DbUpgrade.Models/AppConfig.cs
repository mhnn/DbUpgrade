namespace DbUpgrade.Models
{
    public class AppConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public Dictionary<string, string> ConnectionStrings { get; set; } = null!;
        /// <summary>
        /// 当前环境对应的医院。阿里云对应0，即所有医院。会影响过滤脚本
        /// </summary>
        public string HospitalID { get; set; } = null!;
        /// <summary>
        /// 数据库对应的类型
        /// </summary>
        public Dictionary<string, DbType> DbTypes { get; set; } = null!;
        /// <summary>
        /// 具有版本管理的数据库
        /// </summary>
        public string[] VersionDbs { get; set; } = null!;
        /// <summary>
        /// 执行环境 Developement 开发环境
        /// </summary>
        public Environment? Environment { get; set; }
    }
}
