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


  }
}
