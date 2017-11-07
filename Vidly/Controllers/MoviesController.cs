using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Movie
        public ActionResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre);
            return View(movies);
        }

        public ActionResult Detail(int id)
        {
            Movie movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(c => c.Id == id);
            if (movie == null) return HttpNotFound();
            return View(movie);
        }


        // GET: Movies route
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Die Hard" };
            var customers = new List<Customer>
            {
                new Customer { Name="Customer 1"},
                new Customer {Name = "Customer 2"}
            };
            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movie == null) return HttpNotFound();

            var viewModel = new MovieFormViewModel()
            {
                Movie = movie,
                Genres = _context.Genres.ToList()
            };
            return View("MovieForm", viewModel);
        }

        [Route("movies/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content($"{year}/{month}");
        }

        public ActionResult Save(string s)
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };

            return View("MovieForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Movie movie)
        {
            movie.DateAdded = DateTime.Now;
            if (movie.Id == 0) _context.Movies.Add(movie);
            else
            {
                var movieInDb = _context.Movies.Single(c => c.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.StockNumber = movie.StockNumber;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }
    }
}