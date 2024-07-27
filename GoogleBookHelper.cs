using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GoogleBooksHelper
{
    private readonly BooksService booksService;

    public GoogleBooksHelper(UserCredential credential)
    {
        booksService = new BooksService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "GoogleBooksAPIExample",
        });
    }

    public async Task<List<Book>> FetchBooksAsync()
    {
        var books = new List<Book>();

        var bookshelvesRequest = booksService.Mylibrary.Bookshelves.List();//list the shelves
        var bookshelves = await bookshelvesRequest.ExecuteAsync();

        if (bookshelves.Items != null)
        {
            foreach (var bookshelf in bookshelves.Items)
            {
                Console.WriteLine($"Fetching books from bookshelf: {bookshelf.Title} (ID: {bookshelf.Id})");

                var volumesRequest = booksService.Mylibrary.Bookshelves.Volumes.List(bookshelf.Id.ToString());
                Volumes volumes = null;

                try
                {
                    volumes = await volumesRequest.ExecuteAsync();
                }
                catch (Google.GoogleApiException e)
                {
                    Console.WriteLine($"Error fetching books from bookshelf {bookshelf.Title}: {e.Message}");
                    continue;
                }

                if (volumes?.Items != null)
                {
                    foreach (var volume in volumes.Items)
                    {
                        var book = new Book
                        {
                            Title = volume.VolumeInfo.Title,
                            Authors = volume.VolumeInfo.Authors != null ? string.Join(", ", volume.VolumeInfo.Authors) : "Unknown",
                            CoverPath = volume.VolumeInfo.ImageLinks?.Thumbnail
                        };
                        books.Add(book);
                    }
                }
                else
                {
                    Console.WriteLine($"No books found in bookshelf: {bookshelf.Title}");
                }
            }
        }
        else
        {
            Console.WriteLine("No bookshelves found in the user's library.");
        }

        Console.WriteLine($"Fetched {books.Count} books from Google Books API");

        return books;
    }
}

public class Book
{
    public string Title { get; set; }
    public string Authors { get; set; }
    public string CoverPath { get; set; }
}

