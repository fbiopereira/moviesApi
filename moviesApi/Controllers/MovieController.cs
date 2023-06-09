using Microsoft.AspNetCore.Mvc;
using moviesApi.Models;

namespace moviesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController: ControllerBase
{
    private static List<Movie> movies = new List<Movie>();
    private static int id = 0;
    
    [HttpPost]
    public IActionResult AddNewMovie([FromBody] Movie movie)
    {
        movie.Id = ++id; 
        movies.Add(movie);
        return CreatedAtAction(nameof(GetMovieById), 
            new {id = movie.Id}, 
            movie);

    }

    [HttpGet]
    public IEnumerable<Movie> GetMovies([FromQuery] int page=0, [FromQuery] int pageSize = 10)
    {
        return movies.Skip(pageSize * page).Take(pageSize);
            
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieById(int id)
    {
        var movie = movies.FirstOrDefault(movie => movie.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);

    }

}
