namespace ASF.Web
{
    public class ASFOptions
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DBConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBType { get; set; }

        /// <summary>
        /// 是否允许缓存
        /// </summary>
        public bool AllowCache { get; set; }
    }
}
