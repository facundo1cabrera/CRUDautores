using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class ComentarioCreacionDTO
    {
        [Required]
        public string Contenido { get; set; }
    }
}
