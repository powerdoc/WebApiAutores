namespace WebApiAutores.DTOs
{
    public class AuthorDTOWithBooks: AuthorDTO
    {
        public List<BookDTO> books { get; set; }
    }
}
