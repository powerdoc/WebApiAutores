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

            CreateMap<BookCreationDTO, Book>();
            CreateMap<Book, BookDTO>();

            CreateMap<CommentCreationDTO, Comment>();
            CreateMap<Comment, CommentCreationDTO>();
        }
    }
}
