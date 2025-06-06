using Microsoft.AspNetCore.Http;

namespace Dor.Models
{
    public class Complex
    {
        public int Id { get; set; }
        public string? ComplexName { get; set; }
        public string? Type { get; set; }
        public string? LogoPath { get; set; }
        public string? Address { get; set; }
    }
}
