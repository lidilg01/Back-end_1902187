using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBooks.Entidades;

namespace WebApiBooks.Controllers
{
    [ApiController]
    [Route("api/books")] //ruta del controllador
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public BooksController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet] // api/books
        [HttpGet("listado")] // api/books/listado
        [HttpGet("/listado")] // /listado
        public async Task<ActionResult<List<Book>>> Get()
        {
            return await dbContext.Books.Include(x => x.Genre).ToListAsync();
        }

        [HttpGet("primero")] //api/books/primero
        public async Task<ActionResult<Book>> PrimerBook([FromHeader] int valor, [FromQuery] string Book, [FromQuery] int bookId)
        {
            return await dbContext.Books.FirstOrDefaultAsync();
        }

        [HttpGet("primero2")] //api/books/primero2
        public ActionResult<Book> PrimerBookD()
        {
            return new Book() { Title = "TWO" };
        }

        /* [HttpGet("{id:int}")] 
         public async Task<ActionResult<Book>> Get(int id)
         {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

             if (book == null)
             {
                    return NotFound();
             }

             return book;
         }*/

        [HttpGet("{titulo}")]
        public async Task<ActionResult<Book>> Get([FromRoute] string titulo)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Title.Contains(titulo));

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("{id:int}/{param=Hating}")]
        public async Task<ActionResult<Book>> Get(int id, string param)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Book book)
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
