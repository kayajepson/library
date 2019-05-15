using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            List<Author> allAuthors = Author.GetAll();
            return View(allAuthors);
        }

        public IActionResult New()
        {
          return View();
        }

        [HttpPost("/author/create")]
        public IActionResult Create(string authorName)
        {
          Author newAuthor = new Author(authorName);
          newAuthor.Save();
          return RedirectToAction("Index");
        }

        [HttpGet("/author/{authorId}")]
        public IActionResult Show(int authorId)
        {
          Author author = Author.Find(authorId);
          List<Book> allBooks = Book.GetAll();
          List<Book> authorBooks = Author.GetBooks(authorId);
          Dictionary<string, object> dictionary = new Dictionary<string, object>{};
          dictionary.Add("author", author);
          dictionary.Add("books", allBooks);
          dictionary.Add("authorBooks", authorBooks);
          return View(dictionary);
        }

        [HttpPost("/author/{authorId}/addBook")]
        public IActionResult AssignBook(int authorId, string bookId)
        {
          Author.AssignBook(authorId, int.Parse(bookId));
          return RedirectToAction("Show", new{authorId = authorId});
        }
    }
}
