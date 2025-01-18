namespace BookCatalogClient.Models;

public record BookItem
{
    public Guid Id { get; set; } = default;
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Genre { get; set; }
    public DateTime Date { get; set; }
    public string? Summary { get; set; }
}
