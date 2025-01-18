using BookCatalogAPI.Model;
using BookCatalogAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.RateLimiting;
using System.Runtime.CompilerServices;
using BookCatalogAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BooksCatalogContext>(opt => opt.UseInMemoryDatabase("TodoList")
.UseSeeding((ctx, _) => {
        var books = BooksSeeder.SeedBooks();
        ctx.Set<Book>().AddRange(books);
        ctx.SaveChanges();
    })
);

// Add a CORS policy for the client
// Add .AllowCredentials() for apps that use an Identity Provider for authn/z
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddRateLimiter(_ =>
{
    _.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    _.AddFixedWindowLimiter("fixed", c =>
    {
        c.Window = TimeSpan.FromSeconds(10);
        c.PermitLimit = 3;
        c.QueueLimit = 0;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scp = app.Services.CreateScope();
    using var ctx = scp.ServiceProvider.GetService<BooksCatalogContext>();
    ctx?.Database.EnsureCreated();
}

app.UseCors("wasm");

// Set up API endpoints and methods
var todoItems = app.MapGroup("/books");

todoItems.MapGet("/", GetAllBooks);
todoItems.MapGet("/{id}", GetBook);
todoItems.MapPost("/", CreateBook);
todoItems.MapPut("/{id}", UpdateBook);
todoItems.MapDelete("/{id}", DeleteBook);

app.MapHub<BooksHub>("/bookshub");

app.Run();

static async Task<IResult> GetAllBooks(BooksCatalogContext db, string? filterText = null, string? sort = null, int skip = 0, int limit = 10)
{
    var query = db.Books.AsQueryable();
    if (!string.IsNullOrEmpty(filterText))
    {
        query = query.Where(e => e.Title!.Contains(filterText) || e.Author.Contains(filterText) || e.Genre.Contains(filterText));
    }

    if (!string.IsNullOrEmpty(sort) && !sort.StartsWith("__"))
    {
        var sortOrder = sort.Split("__");
        var column = sortOrder[0];
        var order = sortOrder[1];

        if (order.Equals("Ascending"))
        {
            query = query.OrderBy(e => EF.Property<object>(e, column));
        }
        else if (order.Equals("Descending"))
        {
            query.OrderByDescending(e => EF.Property<object>(e, column));
        }
    }

    var total = query.Count();

    query = query.Skip(skip).Take(limit);
    var result = await query.ToListAsync();

    return TypedResults.Ok(new { Result = result, Total = total });
}

static async Task<IResult> GetBook(Guid id, BooksCatalogContext db)
{
    return await db.Books.FindAsync(id) is Book book ? TypedResults.Ok(book) : TypedResults.NotFound();
}

static async Task<IResult> CreateBook(Book book, BooksCatalogContext db)
{
    book.Date = book.Date?.Date;
    db.Books.Add(book);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/books/{book.Id}", book);
}

static async Task<IResult> UpdateBook(Guid id, Book inputBook, BooksCatalogContext db, IHubContext<BooksHub> hub)
{
    var book = await db.Books.FindAsync(id);

    if (book is null)
    {
        return TypedResults.NotFound();
    }

    book.Title = inputBook.Title;
    book.Author = inputBook.Author;
    book.Date = inputBook.Date?.Date;
    book.Summary = inputBook.Summary;

    await db.SaveChangesAsync();
    await hub.Clients.All.SendAsync(BooksHub.BookUpdatedEventName, book);

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteBook(Guid id, BooksCatalogContext db)
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}