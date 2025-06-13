namespace FruitsAppBackEnd.Models
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInMonths { get; set; }
    }
}
