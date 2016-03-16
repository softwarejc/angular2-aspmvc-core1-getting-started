namespace IdentityClient
{
    public static class Constants
    {
        public const string Authority = "https://localhost:44300/"; // Identity Server
        public const string ResourceApi = "http://localhost:4000/"; // API that only can be used with a valid access token issued by our Authority
        public const string UserInfoEndpoint = Authority + "connect/userinfo";
        public const string TokenEndpoint = Authority + "connect/token";
        public const string ClientId = "mvc";
        public const string ClientSecret = "secret";
    }
}