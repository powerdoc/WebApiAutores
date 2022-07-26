using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;
using WebApiAutores.Filters;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet] //api/authors
        public async Task<ActionResult<List<AuthorDTO>>> Get()
        {
            var authors = await context.Authors/*.Include(x => x.Books)*/.ToListAsync();
            //var authors = await context.Authors.Include(a => a.AuthorBooks).ThenInclude(b => b.Book).ToListAsync();
            return mapper.Map<List<AuthorDTO>>(authors);
        }

        [HttpGet("{id:int}", Name = "GetAuthor")]
        //public async Task<ActionResult<AuthorDTO>> Get(int id)
        public async Task<ActionResult<AuthorDTOWithBooks>> Get(int id)
        {
            var author = await context.Authors.Include(a => a.AuthorBooks).ThenInclude(b => b.Book).FirstOrDefaultAsync(x => x.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return mapper.Map<AuthorDTOWithBooks>(author);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<AuthorDTO>>> Get([FromRoute] string name)
        {
            var authors = await context.Authors.Where(x => x.Name.Contains(name)).ToListAsync();

            return mapper.Map<List<AuthorDTO>>(authors);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AuthorCreationDTO authorCreationDTO)
        {

            bool AuthorWithSameNameIsAlreadyExists = await context.Authors.AnyAsync(x => x.Name == authorCreationDTO.Name);

            if (AuthorWithSameNameIsAlreadyExists)
            {
                return BadRequest($"There already exists an author with the name {authorCreationDTO.Name}");
            }

            var author = mapper.Map<Author>(authorCreationDTO);

            context.Add(author);
            await context.SaveChangesAsync();

            //return Ok();

            var autorDTO = mapper.Map<AuthorDTO>(author);

            return CreatedAtRoute("GetAuthor", new { id = author.Id }, autorDTO);
        }

        [HttpPut("{id:int}")] // api/authors/1
        public async Task<ActionResult> Put(AuthorCreationDTO authorCreationDTO, int id)
        {
            //if (author.Id != id)
            //{
            //    return BadRequest("El id del author no coincide con el id de la URL");
            //}

            var exists = await context.Authors.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            var author = mapper.Map<Author>(authorCreationDTO);
            author.Id = id;

            context.Update(author);
            await context.SaveChangesAsync();
            //return Ok();
            return NoContent();
        }

        [HttpDelete("{id:int}")] // api/authors/2
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Authors.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Author() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
