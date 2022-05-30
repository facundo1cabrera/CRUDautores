using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Entidades;

namespace WebApplication1.Utils
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();

            CreateMap<Autor, AutorDTO>();

            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(libro => libro.Libros, opciones => opciones.MapFrom(MapAutoresDTOLibros));

            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));

            CreateMap<Libro, LibroDTO>();

            CreateMap<Libro, LibroConAutoresDTO>()
                .ForMember(libro => libro.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<LibroDTO> MapAutoresDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();

            if (autor.AutoresLibros == null ) { return resultado; }

            foreach (var libro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO
                {
                    Id = libro.LibroId,
                    Titulo = libro.Libro.Titulo
                });
            }

            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if (libroDTO == null) { return resultado; }

            foreach (var autorlibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorlibro.AutorId,
                    Nombre = autorlibro.Autor.Nombre
                });
            }

            return resultado;
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if ( libroCreacionDTO == null) { return resultado; }

            foreach ( var autorId in libroCreacionDTO.AutoresIds )
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;
        }
    }
}
