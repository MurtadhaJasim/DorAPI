namespace Dor
{
    public class jwtOptions
    {

        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Signingkey { get; set; } = string.Empty;
        public int LifeTime { get; set; }

    }

}
