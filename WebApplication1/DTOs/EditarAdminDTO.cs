using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
