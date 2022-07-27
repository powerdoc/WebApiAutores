using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AuthorCreationDTO, Author>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<Author, AuthorDTOWithBooks>().ForMember(a => a.books, options => options.MapFrom(MapAuthorDTOBooks));

            CreateMap<BookCreationDTO, Book>()
                .ForMember(b => b.AuthorBooks, options => options.MapFrom(MapAuthorBooks));

            CreateMap<Book, BookDTO>();
            CreateMap<Book, BookDTOWithAuthors>().ForMember(l => l.Authors, options => options.MapFrom(MapBookDTOAuthors));

            CreateMap<BookPatchDTO, Book>().ReverseMap();

            CreateMap<CommentCreationDTO, Comment>();
            CreateMap<Comment, CommentDTO>();

        }

        private List<BookDTO> MapAuthorDTOBooks(Author author, AuthorDTO authorDTO)
        {
            var result = new List<BookDTO>();

            if (author.AuthorBooks == null)
            {
                return result;
            }

            foreach (var authorBook in author.AuthorBooks)
            {
                result.Add(new BookDTO()
                {
                    Id = authorBook.BookId,
                    Title = authorBook.Book.Title
                });
            }

            return result;
        }

        private List<AuthorDTO> MapBookDTOAuthors(Book book, BookDTO bookDTO)
        {
            var result = new List<AuthorDTO>();
            if (book.AuthorBooks == null)
            {
                return result;
            }

            foreach (var authorBook in book.AuthorBooks)
            {
                result.Add(new AuthorDTO()
                {
                    Id = authorBook.AuthorId,
                    Name = authorBook.Author.Name
                });
            }

            return result;
        }

        private List<AuthorBook> MapAuthorBooks(BookCreationDTO bookCreationDTO, Book book)
        {
            var result = new List<AuthorBook>();

            if (bookCreationDTO.AuthorIds == null)
            {
                return result;
            }

            foreach (var authorId in bookCreationDTO.AuthorIds)
            {
                result.Add(new AuthorBook() { AuthorId = authorId });
            }

            return result;
        }

    }
}
