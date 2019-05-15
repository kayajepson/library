using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Library.Models
{
  public class Patron
  {
    public string Name {get; set;}
    public int Id {get; set;}
    public string CheckoutDate {get; set;}

    public Patron(string name, string checkoutDate, int id = 0)
    {
      this.Name = name;
      this.CheckoutDate = checkoutDate;
      this.Id = id;
    }

    // public static void ClearAll()
    // {
    //
    // }

    public static void Completed(int patronId, int bookId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE patron_books SET completed = true WHERE (book_id, patron_id) = (@BookId, @PatronId);";
      MySqlParameter bookIdParameter = new MySqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = bookId;
      cmd.Parameters.Add(bookIdParameter);
      cmd.Parameters.AddWithValue("@patronId", patronId);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }



    public static List<Book> BooksCompleted(int patronId)
    {
      List<Book> readBooks = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT *
      FROM patron_books JOIN books
      WHERE patron_books.completed = true and patron_books.patron_id = @patronId and books.id = patron_books.book_id;";
      cmd.Parameters.AddWithValue("@patronId", patronId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        string bookTitle = rdr.GetString(5);
        Book newBook = new Book(bookTitle);
        readBooks.Add(newBook);
      }
      conn.Close();
      return readBooks;
    }

    public static Patron Find(int patronId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patron WHERE id = @patronId;";
      cmd.Parameters.AddWithValue("@patronId", patronId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      rdr.Read();
      int id = rdr.GetInt32(0);
      string name = rdr.GetString(1);
      string checkout = rdr.GetString(2);
      Patron foundPatron = new Patron(name, checkout, id);
      conn.Close();
      return foundPatron;
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patron;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {

        int id = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        string checkoutDate = rdr.GetString(2);
        Patron newPatron = new Patron(patronName, checkoutDate, id);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      return allPatrons;
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
      cmd.CommandText = @"INSERT INTO patron (name, checkoutDate) VALUES (@patronName, @checkoutDate);";
      cmd.Parameters.AddWithValue("@patronName", this.Name);
      cmd.Parameters.AddWithValue("@checkoutDate", this.CheckoutDate);
      cmd.ExecuteNonQuery();
      this.Id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public static void AssignBook(int patronId, int bookId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patron_books (patron_id, book_id) VALUES (@patronId, @bookId);";
      cmd.Parameters.AddWithValue("@patronId", patronId);
      cmd.Parameters.AddWithValue("@bookId", bookId);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Book> GetBooks(int patronId)
    {
      List<Book> patronBooks = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT books.* FROM patron JOIN patron_books ON (patron.id = patron_books.patron_id) JOIN books ON (patron_books.book_id = books.id) WHERE patron.id = @patronId;";
      cmd.Parameters.AddWithValue("@patronId", patronId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {

        int id = rdr.GetInt32(0);
        string bookName = rdr.GetString(1);
        Book newBook = new Book(bookName, id);
        patronBooks.Add(newBook);
      }
      conn.Close();
      return patronBooks;
    }



    // public List<Client> GetClients()
    // {
    //   MySqlConnection conn = DB.Connection();
    //   conn.Open();
    //   MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
    //   cmd.CommandText = @"SELECT * FROM clients WHERE stylist_id = @id;";
    //   cmd.Parameters.AddWithValue("@id", _id);
    //   List<Client> clients = new List<Client>{};
    //   MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
    //   while(rdr.Read())
    //   {
    //     int id = rdr.GetInt32(0);
    //     string name = rdr.GetString(1);
    //     string phone = rdr.GetString(2);
    //     int stylist_id = rdr.GetInt32(3);
    //     Client newClient = new Client(name, phone, stylist_id);
    //     newClient.SetId(id);
    //     clients.Add(newClient);
    //   }
    //   conn.Close();
    //   if (conn != null)
    //   {
    //     conn.Dispose();
    //   }
    //   return clients;
    // }
  }
}
