﻿using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
