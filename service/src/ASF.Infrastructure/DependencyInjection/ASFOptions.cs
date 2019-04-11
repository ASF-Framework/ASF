namespace ASF.Infrastructure.DependencyInjection
{
    public class ASFOptions
    {
        public ASFOptions()
        {
            this.JwtToken = new TokenOptions();
        }
        public TokenOptions JwtToken { get; set; }
        /// <summary>
        /// Sqlite 数据库连接字符串
        /// </summary>
        public string DbConnectionString { get; set; }
    }
}
