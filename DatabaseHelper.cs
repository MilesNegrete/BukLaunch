using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

public class DatabaseHelper
{
    private string connectionString;

    public DatabaseHelper(string dbPath)
    {
        connectionString = $"Data Source={dbPath};Version=3;";
        InitializeDatabase();
    }

    public void InitializeDatabase()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS books (
                    id INTEGER PRIMARY KEY,
                    title TEXT NOT NULL,
                    authors TEXT NOT NULL,
                    cover_path TEXT
                );";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public void AddBook(Book book)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO books (title, authors, cover_path) VALUES (@title, @authors, @coverPath);";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@title", book.Title);
                command.Parameters.AddWithValue("@authors", book.Authors);
                command.Parameters.AddWithValue("@coverPath", book.CoverPath);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Book> GetBooks()
    {
        var books = new List<Book>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string selectQuery = "SELECT title, authors, cover_path FROM books;";
            using (var command = new SQLiteCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Title = reader["title"].ToString(),
                            Authors = reader["authors"].ToString(),
                            CoverPath = reader["cover_path"].ToString()
                        });
                    }
                }
            }
        }
        return books;
    }

    public void ExportDatabaseToCsv(string filePath)
    {
        var books = GetBooks();
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(books);
        }
    }
}

