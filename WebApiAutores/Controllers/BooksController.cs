using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDTO>> Get(int id)
        {
            var book = await context.Books/*.Include(x => x.Author)*/.FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<BookDTO>(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(BookCreationDTO bookCreatingDTO)
        {
            //var exists = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);
            //if (!exists)
            //{
            //    return BadRequest($"No existe el author del Id = {book.AuthorId}");
            //}

            var book = mapper.Map<Book>(bookCreatingDTO);

            context.Add(book);
            await context.SaveChangesAsync();
            return Ok();

        }

    }
}
