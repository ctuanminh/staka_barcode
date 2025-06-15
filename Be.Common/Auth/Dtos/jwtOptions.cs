namespace Be.Common.Auth.Dtos
{
    public class JwtOptions
    {
        public int ExpiresInMinutes { get; set; }
        public string Key { get; set; }
    }
}
