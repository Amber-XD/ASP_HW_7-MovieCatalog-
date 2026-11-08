using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;

namespace MovieCatalog.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private static List<Movie> movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Interstellar", Genre = "Sci-Fi", Year = 2014, Rating = 8.7 },
            new Movie { Id = 2, Title = "The Dark Knight", Genre = "Action", Year = 2008, Rating = 9.0 },
            new Movie { Id = 3, Title = "Inception", Genre = "Sci-Fi", Year = 2010, Rating = 8.8 },
            new Movie { Id = 4, Title = "The Godfather", Genre = "Drama", Year = 1972, Rating = 9.2 }
        };

        // GET /api/movies
        [HttpGet]
        public ActionResult<List<Movie>> GetAll()
        {
            return Ok(movies);
        }

        // GET /api/movies/5
        [HttpGet("{id:int}")]
        public ActionResult<Movie> GetById(int id)
        {
            Movie? movie = movies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        // GET /api/movies/by-title/Interstellar
        [HttpGet("by-title/{title}")]
        public ActionResult<Movie> GetByTitle(string title)
        {
            Movie? movie = movies.FirstOrDefault(
                m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        // GET /api/movies/search?genre=Sci-Fi&minRating=8
        [HttpGet("search")]
        public ActionResult<List<Movie>> Search(
            [FromQuery] string? genre,
            [FromQuery] double? minRating)
        {
            IEnumerable<Movie> result = movies;

            if (!string.IsNullOrEmpty(genre))
            {
                result = result.Where(
                    m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
            }

            if (minRating.HasValue)
            {
                result = result.Where(m => m.Rating >= minRating.Value);
            }

            return Ok(result.ToList());
        }

        // POST /api/movies
        [HttpPost]
        public ActionResult<Movie> Create(Movie movie)
        {
            movie.Id = movies.Count > 0 ? movies.Max(m => m.Id) + 1 : 1;

            movies.Add(movie);

            return CreatedAtAction(
                nameof(GetById),
                new { id = movie.Id },
                movie);
        }

        // DELETE /api/movies/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Movie? movie = movies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            movies.Remove(movie);

            return NoContent();
        }

    }
}
