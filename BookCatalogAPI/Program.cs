using BookCatalogAPI.Model;
using BookCatalogAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BooksCatalogContext>(opt => opt.UseInMemoryDatabase("TodoList"));

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

static async Task<IResult> GetAllBooks(BooksCatalogContext db)
{
    return TypedResults.Ok(await db.Books.ToArrayAsync());
}

static async Task<IResult> GetBook(Guid id, BooksCatalogContext db)
{
    return await db.Books.FindAsync(id) is Book book ? TypedResults.Ok(book) : TypedResults.NotFound();
}

static async Task<IResult> CreateBook(Book book, BooksCatalogContext db)
{
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

    book.Name = inputBook.Name;
    book.Author = inputBook.Author;
    book.Date = inputBook.Date;
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