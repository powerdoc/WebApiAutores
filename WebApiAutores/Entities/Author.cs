using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validations;

namespace WebApiAutores.Entities
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe de tener más de {1} caracteres!")]
        [FirstLetterUpperCase]
        public string Name { get; set; }

        public List<AuthorBook> AuthorBooks { get; set; }
    }
}
