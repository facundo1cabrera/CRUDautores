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

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

       [HttpGet]
       [AllowAnonymous]
       public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}", Name = "obtenerAutor")]
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

            return Ok(autorDTO);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> GetByName([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorBd => autorBd.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost]
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
        [HttpPut("{id:int}")]
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
       [HttpDelete("{id:int}")]
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
