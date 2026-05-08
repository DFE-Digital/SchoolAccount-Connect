using System.ComponentModel.DataAnnotations;

namespace SchoolAccount.Integration.AcademiesApi.Configuration
{
    public class AcademiesApiConfig
    {
        [Required]
        public string PublicUrl { get; set; } = string.Empty;

        [Required]
        public string ApiKey { get; set; } = string.Empty;
    }
}
