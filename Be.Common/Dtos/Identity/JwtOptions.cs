namespace Be.Common.Dtos.Identity
{
    public class JwtOptions
    {
        public string Key { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}
