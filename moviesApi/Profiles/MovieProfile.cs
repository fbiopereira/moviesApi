using AutoMapper;
using moviesApi.Data.Dto;
using moviesApi.Models;

namespace moviesApi.Profiles;

public class MovieProfile: Profile

{
    public MovieProfile()
    {
        CreateMap<CreateMovieDto, Movie>();
    }

}
