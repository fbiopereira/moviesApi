using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using moviesApi.Data;
using moviesApi.Data.Dto;
using moviesApi.Data.Dtos;
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

    /// <summary>
    /// Add a new movie to the database
    /// </summary>
    /// <param name="movieDto">Object with necessary properties to create a movie</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">In the case of movie creation success</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddNewMovie([FromBody] CreateMovieDto movieDto)
    {
        Movie movie = _mapper.Map<Movie>(movieDto);
        _context.Movies.Add(movie);
        _context.SaveChanges(); 
        
        return CreatedAtAction(nameof(GetMovieById), 
            new {id = movie.Id}, 
            movie);
    }

    /// <summary>
    /// Get a movie list from database
    /// </summary>
    /// <param name="page">Desired page number. Default=0</param>
    /// <param name="pageSize">Number of itens per page. Default=10</param>
    /// <returns>Movie list</returns>
    /// <response code="200">Movie list returned from database</response>
    [HttpGet]
    public IEnumerable<ReadMovieDto> GetMovies([FromQuery] int page=0, [FromQuery] int pageSize = 10)
    {
        return _mapper.Map<List<ReadMovieDto>>(_context.Movies.Skip(pageSize * page).Take(pageSize));
            
    }

    /// <summary>
    /// Return a specific movie from database using given ID
    /// </summary>
    /// <param name="id">Movie ID to be retrived from database</param>
    /// <returns>Movie information</returns>
    /// <response code="200">In case of given ID exists at database</response>
    /// <response code="404">ID not found at database</response>
    [HttpGet("{id}")]
    public IActionResult GetMovieById(int id)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
        
        if (movie == null)
        {
            return NotFound();
        }

        var readMovieDto = _mapper.Map<ReadMovieDto>(movie);

        return Ok(readMovieDto);

    }

    /// <summary>
    /// Updates a movie at database using given ID
    /// </summary>
    /// <param name="id">Movie ID to be updated</param>
    /// <param name="updateMovieDto">Object to be used in movie update operation</param>
    /// <returns>No content</returns>
    /// <response code="204">ID found and movie updated</response>
    /// <response code="404">ID not found</response>
    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
      
        if (movie == null)
        {
            return NotFound();
        }
        _mapper.Map(movieDto, movie);      
        _context.SaveChanges();
        return NoContent();
        
    }

    /// <summary>
    /// Partially updates a movie at database using given ID
    /// </summary>
    /// <param name="id">Movie ID to be updated</param>
    /// <param name="UpdateMovieDto">Object to be used in movie update operation</param>
    /// <returns>No content</returns>
    /// <response code="204">ID found and movie updated</response>
    /// <response code="404">ID not found</response>
    [HttpPatch("{id}")]
    public IActionResult UpdateMoviePartially(int id, JsonPatchDocument<UpdateMovieDto> patch)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        var movieToUpdate = _mapper.Map<UpdateMovieDto>(movie);

        patch.ApplyTo(movieToUpdate, ModelState);
        
        if (!TryValidateModel(movieToUpdate))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(movieToUpdate, movie);
        _context.SaveChanges();
        return NoContent();

    }

    /// <summary>
    /// Deletes a movie from database
    /// </summary>
    /// <param name="id">Movie ID to be removed</param>
    /// <returns>No content</returns>
    /// <response code="204">ID found and movie removed</response>
    /// <response code="404">Movie ID not found</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id)
    {
        var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        var readMovieDto = _mapper.Map<ReadMovieDto>(movie);
        _context.Remove(movie);
        _context.SaveChanges();
        return Ok(readMovieDto);

    }

}
