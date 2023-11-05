using MovieInfoSearcher.Services;
using RestSharp;
using System.Text.Json;

namespace MovieInfoSearcher.Models
{
    class MovieModel
    {
        public string original_title { get; set; }
        public string overview { get; set; }
        public List<int> genre_ids { get; set; }
        public DateOnly? release_date { get; set; }

        public double vote_average { get; set; }


        public async Task<string> getGenresString()
        {
            string result = string.Empty;
            MovieSeracherService movieSeracherService = new MovieSeracherService();
            var genreListJson = movieSeracherService.MakeRequestToTMDB(new RestClientOptions("https://api.themoviedb.org/3/genre/movie/list?language=en")).Result.RootElement.GetProperty("genres");

            List<GenreModel> genres = genreListJson.Deserialize<List<GenreModel>>();

            for (int i = 0; i < genre_ids.Count - 1; i++)
            {
                result += genres.Where(x => x.id == genre_ids[i]).FirstOrDefault().name + ", ";
            }
            result += genres.Where(x => x.id == genre_ids[genre_ids.Count - 1]).FirstOrDefault().name;

            return result;
        }
    }
}
