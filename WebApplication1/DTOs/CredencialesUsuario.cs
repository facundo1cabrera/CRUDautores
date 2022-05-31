using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }
    }
}
