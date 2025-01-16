namespace BookCatalogClient.Models;

public record BookItem
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Author { get; set; }
    public DateTime Date { get; set; }
    public string? Summary { get; set; }
}
