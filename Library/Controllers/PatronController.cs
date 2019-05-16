using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
  public class PatronController : Controller
  {
    public IActionResult Index()
    {
      List<Patron> allPatrons = Patron.GetAll();
      // List<Patron> allPatrons = new List<Patron> {};
      // Patron newPatron = new Patron("patronName", "checkoutDate", 23);
      // allPatrons.Add(newPatron);
      return View(allPatrons);
    }

    public IActionResult New()
    {
      return View();
    }

    [HttpPost("/patron/create")]
    public IActionResult Create(string patronName, string checkoutDate)
    {
      Patron newPatron = new Patron(patronName, checkoutDate);
      newPatron.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/patron/{patronId}")]
    public IActionResult Show(int patronId)
    {
      Patron patron = Patron.Find(patronId);
      List<Book> allBooks = Book.GetAll();
      List<Book> patronBooks = Patron.GetBooks(patronId);
      List<Book> readBooks = Patron.BooksCompleted(patronId);
      Dictionary<string, object> dictionary = new Dictionary<string, object>{};
      dictionary.Add("patron", patron);
      dictionary.Add("books", allBooks);
      dictionary.Add("patronBooks", patronBooks);
      dictionary.Add("readBooks", readBooks);
      return View(dictionary);
    }

    [HttpPost("/patron/{patronId}/remove-book")]
    public ActionResult RemoveBook(int bookId, int patronId)
    {
      Book book = Book.Find(bookId);
      book.Remove();
      return RedirectToAction("Show", new{patronId = patronId});
    }

    [HttpPost("/patron/{patronId}/addBook")]
    public IActionResult AssignBook(int patronId, string bookId)
    {
      Patron.AssignBook(patronId, int.Parse(bookId));
      return RedirectToAction("Show", new{patronId = patronId});
    }

    [HttpPost("/patron/{id}/completed")]
    public ActionResult Completed(int patronId, int bookId)
    {
      Patron.Completed(patronId, bookId);
      return RedirectToAction("Show", new{patronId = patronId});
    }


  }
}
