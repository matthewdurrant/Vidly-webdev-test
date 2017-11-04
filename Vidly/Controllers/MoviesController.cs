using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies route
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Die Hard" };
            return View(movie);
        }

        public ActionResult Edit(int id)
        {
            return Content("You entered id " + id);
        }
    }
}