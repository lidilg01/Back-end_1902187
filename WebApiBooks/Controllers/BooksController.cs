using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBooks.Entidades;
using Microsoft.AspNetCore.Authorization;
using WebApiBooks.Filtros;
using WebApiBooks.Services;


namespace WebApiBooks.Controllers
{
    [ApiController]
    [Route("api/books")] //ruta del controlador
    public class BooksController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly string archivoPost = "nuevoRegistros.txt";
        private readonly string archivoGet = "registroConsultado.txt";
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<BooksController> logger;
        public BooksController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<BooksController> logger, IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }

        [HttpGet("GUID")]
        //[ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
            return Ok(new
            {
                BooksControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                BooksControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                BooksControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }

        [HttpGet] // api/books
        [HttpGet("listado")] // api/books/listado
        [HttpGet("/listado")] // /listado
        //[ResponseCache(Duration = 10)]
        //[Authorize]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public async Task<ActionResult<List<Book>>> Get()
        {
            throw new NotImplementedException();
            logger.LogInformation("Se obtiene el listado de alumnos");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
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

            var ruta = $@"{env.ContentRootPath}\wwwroot\{archivoGet}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine($@"{book.Id},{book.Title}"); }

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
            var existeLibroMismoNombre = await dbContext.Books.AnyAsync(x => x.Title == book.Title);

            if (existeLibroMismoNombre)
            {
                return BadRequest("Ya existe un autor con el nombre");
            }

            var ruta = $@"{env.ContentRootPath}\wwwroot\{archivoPost}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine($@"{book.Id},{book.Title}"); }
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
