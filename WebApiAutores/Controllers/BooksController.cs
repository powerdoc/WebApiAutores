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

        [HttpGet("{id:int}", Name = "GetBook")]
        //public async Task<ActionResult<BookDTO>> Get(int id)
        public async Task<ActionResult<BookDTOWithAuthors>> Get(int id)
        {
            var book = await context.Books.Include(l => l.AuthorBooks).ThenInclude(a => a.Author).FirstOrDefaultAsync(x => x.Id == id);
            book.AuthorBooks = book.AuthorBooks.OrderBy(x => x.Order).ToList();
            return mapper.Map<BookDTOWithAuthors>(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(BookCreationDTO bookCreatingDTO)
        {
            //var exists = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);
            //if (!exists)
            //{
            //    return BadRequest($"No existe el author del Id = {book.AuthorId}");
            //}

            if (bookCreatingDTO.AuthorIds == null)
            {
                return BadRequest("It's not possible to create a book without authors!");
            }

            var authorIds = await context.Authors.Where(a => bookCreatingDTO.AuthorIds.Contains(a.Id)).Select(x => x.Id).ToListAsync();
            if (bookCreatingDTO.AuthorIds.Count != authorIds.Count)
            {
                return BadRequest("Some of the sent authors don't exist!");
            }

            var book = mapper.Map<Book>(bookCreatingDTO);

            if (book.AuthorBooks != null)
            {
                for (int i = 0; i < book.AuthorBooks.Count; i++)
                {
                    book.AuthorBooks[i].Order = i;
                }
            }

            context.Add(book);
            await context.SaveChangesAsync();
            //return Ok();

            var bookDto = mapper.Map<BookDTO>(book);

            return CreatedAtRoute("GetBook", new { Id = book.Id }, bookDto);
        }

    }
}
