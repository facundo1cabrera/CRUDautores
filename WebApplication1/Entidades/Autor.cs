using System.ComponentModel.DataAnnotations;
using WebApplication1.Validations;

namespace WebApplication1.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo nombre es requerido")]
        [StringLength(100, ErrorMessage = "Maxima cantidad de caracteres {0}")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }

    }
}
