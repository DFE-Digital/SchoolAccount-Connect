using System.ComponentModel.DataAnnotations;

namespace SchoolAccount.Integration.DfESignIn.Configuration
{
    public class DsiApiConfig
    {
        [Required]
        public string ApiSecret { get; set; } = string.Empty;
        [Required]
        public string ServiceAudience { get; set; } = string.Empty;
        [Required]
        public string Issuer { get; set; } = string.Empty;
        [Required]
        public string PublicUrl { get; set; } = string.Empty;
    }
}
