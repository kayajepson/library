using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/")]
        public ActionResult Search(int id)
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
