using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBooks.Entidades;

namespace WebApiBooks.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public BooksController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            return await dbContext.Books.Include(x => x.Genre).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Book book)
        {
            dbContext.Add(book);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Book book, int id)
        {
            if (book.Id != id)
            {
                return BadRequest("El id del libro no coicide con el establecido en la URL");
            }

            dbContext.Update(book);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Books.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            dbContext.Remove(new Book()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
