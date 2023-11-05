using RestSharp;
using System.Text.Json;
using MovieInfoSearcher.Models;
using System.Text.RegularExpressions;

namespace MovieInfoSearcher.Services
{
    public interface IMovieSearcherService
    {
        public Task<string> GetPopular(int count);
        public Task<string> GetPlayingNow(string region);
        public Task<JsonDocument> MakeRequestToTMDB(RestClientOptions options);
        public string formStringResult(JsonElement movieList, int count);
    }

    public class MovieSeracherService : IMovieSearcherService
    {
        public async Task<JsonDocument> MakeRequestToTMDB(RestClientOptions options)
        {
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJiZjNlOWFlOGY1Mjk0NjYxOWM4MTdlNWVkOWZhYjMxNCIsInN1YiI6IjY1NDc3ZjM0ZDhjYzRhMDBhZDIzOTM3YiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.WHqvQRUVgMqq6u6P7GSxA25WXQTIMUWdjEC9cSxQfpk");
            var response = await client.GetAsync(request);
            var json = JsonDocument.Parse(response.Content);
            return json;
        }



        public async Task<string> GetPopular(int count)
        {

            var response = MakeRequestToTMDB(new RestClientOptions("https://api.themoviedb.org/3/movie/popular?language=en-US&page=1")).Result;

            var result = response.RootElement.GetProperty("results");

            return formStringResult(result, count);

        }


        public async Task<string> GetPlayingNow(string region)
        {

            var response = MakeRequestToTMDB(new RestClientOptions($"https://api.themoviedb.org/3/movie/now_playing?language=en-US&page=1&region={region}")).Result;

            var result = response.RootElement.GetProperty("results");

            return formStringResult(result);

        }

        public string formStringResult(JsonElement movieList, int count = 10)
        {

            string messageResult = "";
            for (var i = 0; i < count; i++)
            {
                MovieModel? movie = JsonSerializer.Deserialize<MovieModel>(movieList[i]);

                messageResult += $"🎬 *{movie.original_title}*" + "\n" + $"⭐️{movie.vote_average.ToString("#.##")}" + "\n" + $"📅 {movie.release_date.Value.Year} \n" + "\n" + $"✍️ {movie.overview}" + "\n \n" + movie.getGenresString().Result + "\n \n";
            }
            return messageResult;
        }
    }
}
