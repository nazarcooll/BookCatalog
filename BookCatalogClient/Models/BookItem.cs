namespace BookCatalogClient.Models;

public record BookItem
{
    public Guid Id { get; set; } = default;
    public string? Name { get; set; }
    public string? Author { get; set; }
    public DateTime Date { get; set; }
    public string? Summary { get; set; }
}
