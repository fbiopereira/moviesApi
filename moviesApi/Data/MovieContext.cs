using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using moviesApi.Models;

namespace moviesApi.Data;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> opts)  
        :base(opts)
    {          

    }

    public DbSet<Movie> Movies { get; set; }
}
