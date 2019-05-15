using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Author
  {
    public string Name {get; set;}
    public int Id {get; set;}

    public Author(string name, int id = 0)
    {
      this.Name = name;
      this.Id = id;
    }

    // public static void ClearAll()
    // {
    //
    // }

    public static Author Find(int authorId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM author WHERE id = @authorId;";
      cmd.Parameters.AddWithValue("@authorId", authorId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      rdr.Read();
      int id = rdr.GetInt32(0);
      string name = rdr.GetString(1);
      Author foundAuthor = new Author(name, id);
      conn.Close();
      return foundAuthor;
    }

    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM author;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {

        int id = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author(authorName, id);
        allAuthors.Add(newAuthor);
        }
      conn.Close();
      return allAuthors;
    }

    public static void AssignBook(int authorId, int bookId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO author_books (author_id, book_id) VALUES (@authorId, @bookId);";
      cmd.Parameters.AddWithValue("@authorId", authorId);
      cmd.Parameters.AddWithValue("@bookId", bookId);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Book> GetBooks(int authorId)
    {
      List<Book> authorBooks = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT books.* FROM author JOIN author_books ON (author.id = author_books.author_id) JOIN books ON (author_books.book_id = books.id) WHERE author.id = @authorId;";
      cmd.Parameters.AddWithValue("@authorId", authorId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {

        int id = rdr.GetInt32(0);
        string bookName = rdr.GetString(1);
        Book newBook = new Book(bookName, id);
        authorBooks.Add(newBook);
        }
      conn.Close();
      return authorBooks;
    }

    // public override bool Equals(System.Object otherAuthor)
    // {
    //   if (!(otherAuthor is Author))
    //   {
    //     return false;
    //   }
    //   else
    //   {
    //     Author newAuthor = (Author) otherAuthor;
    //     bool idEquality = (this.GetId() == newAuthor.GetId());
    //     bool nameEquality = (this.GetAuthorName() == newAuthor.GetAuthorName());
    //     return (idEquality && nameEquality);
    //   }
    // }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO author (name) VALUES (@authorName);";
      cmd.Parameters.AddWithValue("@authorName", this.Name);
      cmd.ExecuteNonQuery();
      this.Id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    // public List<Book> GetBooks()
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"SELECT * FROM books WHERE author_id = @id;";
    //   cmd.Parameters.AddWithValue("@id", _id);
    //   List<Book> books = new List<Book>{};
    //   MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
    //   while(rdr.Read())
    //   {
    //     int id = rdr.GetInt32(0);
    //     string name = rdr.GetString(1);
    //     string phone = rdr.GetString(2);
    //     int author_id = rdr.GetInt32(3);
    //     Book newBook = new Book(name, phone, author_id);
    //     newBook.SetId(id);
    //     books.Add(newBook);
    //   }
    //   conn.Close();
    //   if (conn != null)
    //   {
    //     conn.Dispose();
    //   }
    //   return books;
    // }
  }
}
