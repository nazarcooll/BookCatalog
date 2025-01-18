using BookCatalogAPI.Model;

namespace BookCatalogAPI;

public static class BooksSeeder
{
    public static List<Book> SeedBooks()
    {
        var titles = new[]
        {
            "The Great Adventure", "Mystery in the Woods", "Journey to the Unknown", "Tales of the Forgotten",
            "The Last Kingdom", "Whispers of the Past", "Echoes of Eternity", "Shadows and Light",
            "Dreams of Tomorrow", "The Silent Watcher"
        };

        var authors = new[]
        {
            "John Doe", "Jane Smith", "Alice Johnson", "Robert Brown",
            "Emily Davis", "Michael Wilson", "Sarah Moore", "James Taylor",
            "Laura Anderson", "David Thomas"
        };

        var genres = new[] { "Fiction", "Mystery", "Fantasy", "Science Fiction", "Historical", "Romance", "Thriller" };

        var summaries = new[]
        {
            "An epic tale of bravery and adventure.",
            "A gripping mystery set in an ancient forest.",
            "A journey that changes everything.",
            "Forgotten tales that resurface in a surprising way.",
            "A battle for a kingdom lost in time.",
            "Whispers of the past that reveal deep secrets.",
            "A timeless story of love and loss.",
            "Light and shadows collide in this thrilling narrative.",
            "Dreams that foretell an uncertain future.",
            "A silent observer with a story to tell."
        };

        var random = new Random();
        var books = new List<Book>();

        for (int i = 0; i < 125; i++)
        {
            books.Add(new Book
            {
                Id = Guid.NewGuid(),
                Title = titles[random.Next(titles.Length)],
                Author = authors[random.Next(authors.Length)],
                Genre = genres[random.Next(genres.Length)],
                Summary = summaries[random.Next(summaries.Length)],
                Date = DateTime.Now.AddDays(-random.Next(0, 10000)).Date // Random date in the past ~27 years
            });
        }

        return books;
    }
}