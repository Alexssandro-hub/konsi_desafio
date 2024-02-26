namespace konsi.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string Type { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
