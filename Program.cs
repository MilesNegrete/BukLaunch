class Program
{
    static async Task Main(string[] args)
    {
        var googleAuthHelper = new GoogleAuthHelper();
        var credential = await googleAuthHelper.AuthenticateAsync();

        var googleBooksHelper = new GoogleBooksHelper(credential);
        var books = await googleBooksHelper.FetchBooksAsync();

        if (books.Count == 0)
        {
            Console.WriteLine("No books fetched from the Google Books API.");
            return;
        }

        var dbPath = "books.db";
        var databaseHelper = new DatabaseHelper(dbPath);

        foreach (var book in books)
        {
            databaseHelper.AddBook(book);
        }

        string exportDirectory = @"C:\Users\miles\Desktop\BookLaunch";
        if (!Directory.Exists(exportDirectory))
        {
            Directory.CreateDirectory(exportDirectory);
        }

        var csvPath = Path.Combine(exportDirectory, "books.csv");
        databaseHelper.ExportDatabaseToCsv(csvPath);

        Console.WriteLine($"Books have been stored in the database and exported to {csvPath}");
    }
}
