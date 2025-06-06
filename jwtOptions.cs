namespace Dor
{
    public class jwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifeTime { get; set; }
        public string Signingkey { get; set; }
    }

}
