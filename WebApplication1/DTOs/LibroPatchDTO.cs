namespace WebApplication1.DTOs
{
    public class LibroPatchDTO
    {
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
