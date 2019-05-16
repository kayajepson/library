using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class BookController : Controller
  {
    public IActionResult Index()
    {
      List<Book> allBooks = Book.GetAll();
      return View(allBooks);
    }

    public IActionResult New()
    {
      return View();
    }

    [HttpPost("/book/create")]
    public IActionResult Create(string bookTitle)
    {
      Book newBook = new Book(bookTitle);
      newBook.Save();
      return RedirectToAction("Index");
    }

    [HttpPost("/books/{bookId}/delete-book")]
    public ActionResult DeleteBook(int bookId)
    {
      Book book = Book.Find(bookId);
      book.Delete();
      // Dictionary<string, object> model = new Dictionary<string, object>();
      // Author foundAuthor = Author.Find(authorId);
      // List<Book> authorBooks = foundAuthor.GetBooks();
      // model.Add("book", authorBooks);
      return RedirectToAction("Index");
    }

    [HttpGet("/book/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Book selectedBook = Book.Find(id);
      List<Author> bookAuthors = selectedBook.GetAuthors(id);
      model.Add("selectedBook", selectedBook);
      model.Add("bookAuthors", bookAuthors);
      return View(model);
    }
  }
}
