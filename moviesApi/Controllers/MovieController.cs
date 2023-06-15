using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using moviesApi.Data;
using moviesApi.Data.Dto;
using moviesApi.Models;

namespace moviesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController: ControllerBase
{
    private static List<Movie> movies = new List<Movie>();
    private static int id = 0;

    private MovieContext _context;
    private IMapper _mapper;

    public MovieController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddNewMovie([FromBody] CreateMovieDto movieDto)
    {
        Movie movie = _mapper.Map<Movie>(movieDto);
        _context.Movies.Add(movie);
        _context.SaveChanges(); 
        
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
