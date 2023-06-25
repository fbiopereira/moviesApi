using moviesApi.Data.Dto;

namespace moviesApi.Data.Dtos
{
    public class ReadMovieDto : MovieDto
    {
        public DateTime ReadDateTime { get; set; } = DateTime.Now;
    }
}
