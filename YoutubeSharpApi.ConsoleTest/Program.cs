using System;
using System.Threading;
using System.Threading.Tasks;
using YoutubeSharpApi.Models.Playlist;
using YoutubeSharpApi.Models.Trending;

namespace YoutubeSharpApi.ConsoleTest
{
    class Program
    {
        private static string _apiKey = "--- YOUR API KEY ---"; // https://developers.google.com/youtube/v3/getting-started
        private static string _playlistId = "PLzByySESNL7GKiOXOs7ew5vEFBxuJvf0D"; // https://www.youtube.com/playlist?list=PLzByySESNL7GKiOXOs7ew5vEFBxuJvf0D
        static void Main(string[] args)
        {
            Console.WriteLine($"--- --- --- --- Playlist --- --- --- ---");
            var youtubeClient = new YoutubeClient(_apiKey);
            var response = Task.Run(() => youtubeClient.GetPlayListAsync(new PlaylistInput
            {
                PlaylistId = _playlistId
            }, CancellationToken.None)).Result;

            foreach (var item in response.VideosData)
            {
                Console.WriteLine($"{item.Position} - {item.Title} (Video Id : {item.VideoId})");
            }



            Console.WriteLine($"--- --- --- --- Trendings --- --- --- ---");
            var responseTrending = Task.Run(() => youtubeClient.GetTrendingAsync(new TrendingInput
            {
                Category = TrendingEnums.Categories.Music,
                Region = TrendingEnums.Regions.Turkey
            }), CancellationToken.None).Result;

            int i = 0;
            foreach (var item in responseTrending.VideosData)
            {
                Console.WriteLine($"{++i} - {item.Title} (Video Id : {item.VideoId}) ({item.CategoryId} | {item.Category.ToString()})");
            }




            Console.WriteLine($"--- --- --- --- Playlist With Pagging --- --- --- ---");
            var playlistInput = new PlaylistInput
            {
                MaxResultPerPage = 20,
                PlaylistId = _playlistId
            };

            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine("Page : " + j);
                var responsePlaylistPaging = Task.Run(() => youtubeClient.GetPlaylistWithPagingAsync(playlistInput, token: new CancellationToken())).Result;
                playlistInput.PageToken = responsePlaylistPaging.NextPage;
                foreach (var item in responsePlaylistPaging.VideosData)
                {
                    Console.WriteLine($"{item.Position} - {item.Title} (Video Id : {item.VideoId})");
                }
            }



            Console.WriteLine($"--- --- --- --- Trending With Pagging --- --- --- ---");
            var trendingInput = new TrendingInput()
            {
                MaxResultPerPage = 20,
                Category = TrendingEnums.Categories.Music,
                Region = TrendingEnums.Regions.Turkey
            };
            int k = 0;
            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine("Page : " + j);
                var responsePlaylistPaging = Task.Run(() => youtubeClient.GetTrendingWithPagingAsync(trendingInput, token: new CancellationToken())).Result;
                playlistInput.PageToken = responsePlaylistPaging.NextPage;
                foreach (var item in responsePlaylistPaging.VideosData)
                {
                    Console.WriteLine($"{++k} - {item.Title} (Video Id : {item.VideoId})");
                }
            }
            Console.ReadKey();
        }
    }
}
