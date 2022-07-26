using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/books/{bookId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(int bookId)
        {
            var bookExists = await context.Books.AnyAsync(x => x.Id == bookId);
            if (!bookExists)
            {
                return NotFound();
            }

            var comments = await context.Comments.Where(c => c.BookId == bookId).ToListAsync();
            return mapper.Map<List<CommentDTO>>(comments);
        }

        [HttpGet("{id:int}", Name = "GetComment")]
        public async Task<ActionResult<CommentDTO>> GetById(int id)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return mapper.Map<CommentDTO>(comment);
        }

        [HttpPost]
        public async Task<ActionResult> Post(int bookId, CommentCreationDTO commentCreationDTO)
        {
            var bookExists = await context.Books.AnyAsync(b => b.Id == bookId);
            if (!bookExists)
            {
                return NotFound();
            }

            var comment = mapper.Map<Comment>(commentCreationDTO);
            comment.BookId = bookId;
            context.Add(comment);
            await context.SaveChangesAsync();
            //return Ok();

            var commentDTO = mapper.Map<CommentDTO>(comment);

            return CreatedAtRoute("GetComment", new { Id = comment.Id, BookId = bookId }, commentDTO);
        }
    }
}
