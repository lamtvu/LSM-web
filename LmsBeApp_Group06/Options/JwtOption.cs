namespace LmsBeApp_Group06.Options
{
    public class JwtOption
    {
        public string TokenSecretKey { get; set; }
        public string RefreshSecretKey { get; set; }
        public string VerifictionSecretKey { get; set; }
        public string InviteSecretKey { get; set; }
        public int InivteLifetime { get; set; }
        public int TokenLifetime { get; set; }
        public int RefreshLifetime { get; set; }
        public int VerificationLifetime { get; set; }
    }
}