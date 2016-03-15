namespace IdentityClient
{
    public static class Constants
    {
        public const string Authority = "https://localhost:44300/"; // Identity Server
        public const string UserInfoEndpoint = Authority + "connect/userinfo";
        public const string TokenEndpoint = Authority + "connect/token";
        public const string ClientId = "mvc";
        public const string ClientSecret = "secret";
    }
}