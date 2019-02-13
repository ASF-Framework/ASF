namespace ASF.Infrastructure.DependencyInjection
{
    public class ASFOptions
    {
        public ASFOptions()
        {
            this.JwtToken = new TokenOptions();
        }
        public TokenOptions JwtToken { get; set; }
    }
}
