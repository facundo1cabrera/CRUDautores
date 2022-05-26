using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
        }

        //[HttpPost]
        //public async Task<ActionResult> Post(Libro libro)
        //{
        //    var existeAutor = await context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);
        //    if (!existeAutor)
        //    {
        //        return BadRequest("El autor no esta registrado");
        //    }

        //    context.Add(libro);
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}
    }
}
