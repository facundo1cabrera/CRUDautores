using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.DTOs;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

       [HttpGet(Name = "obtenerAutores")]
       [AllowAnonymous]
       public async Task<IActionResult> Get([FromQuery] bool incluirHATEOAS = true)
        {
            var autores = await context.Autores.ToListAsync();
            var dtos = mapper.Map<List<AutorDTO>>(autores);

            if(incluirHATEOAS)
            {
                var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

                dtos.ForEach(dto => GenerarEnlaces(dto, esAdmin.Succeeded));

                var resultado = new ColeccionDeRecursos<AutorDTO> { Valores = dtos };
                resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("obtenerAutores", new { }), descripcion: "self",
                    metodo: "GET"));

                if (esAdmin.Succeeded)
                {
                    resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("crearAutor", new { }), descripcion: "crear-autor",
                        metodo: "POST"));
                }

                return Ok(resultado);
            }


            return Ok(dtos);
        }

        [HttpGet("{id:int}", Name = "obtenerAutor")]
        [AllowAnonymous]
        public async Task<ActionResult<AutorDTOConLibros>> GetOne(int id)
        {
            var autor = await context.Autores
                .Include(autor => autor.AutoresLibros)
                .ThenInclude(autoresLibros => autoresLibros.Libro)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorDTOConLibros>(autor);
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
            GenerarEnlaces(autorDTO, esAdmin.Succeeded);
            return Ok(autorDTO);
        }

        private void GenerarEnlaces(AutorDTO autorDTO, bool esAdmin)
        {
            autorDTO.Enlaces.Add(new DatoHATEOAS(Url.Link("obtenerAutor", new { id = autorDTO.Id }),
                descripcion: "self", metodo: "GET"));

            if (esAdmin)
            {
                autorDTO.Enlaces.Add(new DatoHATEOAS(Url.Link("actualizarAutor", new { id = autorDTO.Id }),
                    descripcion: "autor-actualizar", metodo: "PUT"));
                autorDTO.Enlaces.Add(new DatoHATEOAS(Url.Link("borrarAutor", new { id = autorDTO.Id }),
                    descripcion: "autor-borrar", metodo: "DELETE"));
            }
        }

        [HttpGet("{nombre}", Name = "obtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorDTO>>> GetByName([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorBd => autorBd.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost(Name = "crearAutor")]
       public async  Task<ActionResult> Post([FromBody]AutorCreacionDTO autorCreacionDTO)
        {
            var existeConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if(existeConElMismoNombre)
            {
                return BadRequest("Existe un autor con el mismo nombre");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("obtenerAutor", new { id = autor.Id }, autorDTO);   
        }
        [HttpPut("{id:int}", Name = "actualizarAutor")]
       public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {

            var exist = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();

        }
       [HttpDelete("{id:int}", Name = "borrarAutor")]
       public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
