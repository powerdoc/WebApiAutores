﻿using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.DTOs
{
    public class BookCreationDTO
    {
        [FirstLetterUpperCase]
        [StringLength(maximumLength: 250)]
        [Required]
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}
