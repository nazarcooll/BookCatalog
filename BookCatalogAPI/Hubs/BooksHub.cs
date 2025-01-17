using Microsoft.AspNetCore.SignalR;

namespace BookCatalogAPI.Hubs;

public class BooksHub : Hub
{
    public const string BookUpdatedEventName = "BookUpdated";
}
