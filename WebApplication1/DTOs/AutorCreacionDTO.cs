using System.ComponentModel.DataAnnotations;
using WebApplication1.Validations;

namespace WebApplication1.DTOs
{
    public class AutorCreacionDTO
    {
        [Required( ErrorMessage = "Campo nombre no puede ser vacio")]
        [StringLength(100, ErrorMessage = "El campo nombre debe tener maximo 100 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; } 
    }
}
