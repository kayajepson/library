using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Book
  {
    public string Title {get; set;}
    public int Id {get; set;}

    public Book(string bookTitle, int id = 0)
    {
      this.Title = bookTitle;
      this.Id = id;
    }

    public static Book Find(int bookId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id = @bookId;";
      cmd.Parameters.AddWithValue("@bookId", bookId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      rdr.Read();
      int id = rdr.GetInt32(0);
      string bookTitle = rdr.GetString(1);
      Book foundBook = new Book(bookTitle, id);
      conn.Close();
      return foundBook;
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {

        int id = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        Book newBook = new Book(bookTitle, id);
        allBooks.Add(newBook);
      }
      conn.Close();
      return allBooks;
    }

    // public override bool Equals(System.Object otherStylist)
    // {
    //   if (!(otherStylist is Stylist))
    //   {
    //     return false;
    //   }
    //   else
    //   {
    //     Stylist newStylist = (Stylist) otherStylist;
    //     bool idEquality = (this.GetId() == newStylist.GetId());
    //     bool nameEquality = (this.GetStylistName() == newStylist.GetStylistName());
    //     return (idEquality && nameEquality);
    //   }
    // }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO books (bookTitle) VALUES (@bookTitle);";
      cmd.Parameters.AddWithValue("@bookTitle", this.Title);
      cmd.ExecuteNonQuery();
      this.Id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books WHERE id = @BookId; DELETE FROM patron_books WHERE book_id = @BookId;";
      cmd.Parameters.AddWithValue("@BookId", this.Id);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }


    public void Remove()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patron_books WHERE book_id = @BookId;";
      MySqlParameter bookIdParameter = new MySqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.Id;
      cmd.Parameters.Add(bookIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }


    public List<Author> GetAuthors(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT author.* FROM books
      JOIN author_books ON (books.id = author_books.book_id)
      JOIN author ON (author_books.author_id = author.id)
      WHERE books.id = @BookId;";
      cmd.Parameters.AddWithValue("@BookId", id);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Author> authors = new List<Author>{};
      while(rdr.Read())
      {
        int thisAuthorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author foundAuthor = new Author(authorName, thisAuthorId);
        authors.Add(foundAuthor);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return authors;
    }



  }
}
