using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBooks.Entidades;

namespace WebApiBooks.Controllers
{
    [ApiController]
    [Route("api/clases")]
    public class GenreController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public GenreController (ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genre>>> GetAll()
        {
            return await dbContext.Genres.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genre>> GetById(int id)
        {
            return await dbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Genre genre)
        {
            var existeBook = await dbContext.Books.AnyAsync(x => x.Id == genre.BookId);

            if (!existeBook)
            {
                return BadRequest($"No existe el libro con el id: {genre.BookId}");
            }

            dbContext.Add(genre);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Genre genre, int id)
        {
            var exist = await dbContext.Genres.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El genero especificado no existe. ");
            }

            if (genre.Id != id)
            {
                return BadRequest("El id del genero no coincide con el establecido en la url. ");
            }

            dbContext.Update(genre);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Genres.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado");
            }

            dbContext.Remove(new Genre { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
