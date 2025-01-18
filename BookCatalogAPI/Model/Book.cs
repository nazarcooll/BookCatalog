using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalogAPI.Model;

public sealed class Book
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public string? Summary { get; set; }
    public DateTime? Date { get; set; }
}
