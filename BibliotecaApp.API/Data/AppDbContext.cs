using BibliotecaApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApp.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Livro> Livros { get; set; }
}

