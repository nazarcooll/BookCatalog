using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookCatalogAPI.Model;
public class BooksCatalogContext(DbContextOptions<BooksCatalogContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; } = null!;
}
