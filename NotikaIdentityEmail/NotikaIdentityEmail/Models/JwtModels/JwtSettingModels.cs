namespace NotikaIdentityEmail.Models.JwtModels
{
    public class JwtSettingModels
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpireMinutes { get; set; }
    }
}
