using System.ComponentModel.DataAnnotations;

namespace hotelapp_frontend.Models
{
    public class PasswordReset
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
