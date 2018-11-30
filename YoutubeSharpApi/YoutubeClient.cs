using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using YoutubeSharpApi.Models;
using YoutubeSharpApi.Models.Playlist;
using YoutubeSharpApi.Models.Trending;

namespace YoutubeSharpApi
{

    public class YoutubeClient
    {
        private static HttpClient _httpClient;
        private static readonly string _baseUri = "https://www.googleapis.com/youtube/v3/";
        private static string _apiKey;
        public YoutubeClient(string apiKey)
        {
            _apiKey = apiKey;
        }
        public async Task<PlaylistOutput> GetPlayListAsync(PlaylistInput input, CancellationToken token = new CancellationToken())
        {
            return await this.GetPlayListItemsAsync(input, token, false);
        }
        public async Task<PlaylistOutput> GetPlaylistWithPagingAsync(PlaylistInput input, CancellationToken token = new CancellationToken())
        {
            return await this.GetPlayListItemsAsync(input, token, true);
        }
        public async Task<TrendingOutput> GetTrendingAsync(TrendingInput input, CancellationToken token = new CancellationToken())
        {
            return await this.GetTrendingItemsAsync(input, token, false);
        }
        public async Task<TrendingOutput> GetTrendingWithPagingAsync(TrendingInput input, CancellationToken token = new CancellationToken())
        {
            return await this.GetTrendingItemsAsync(input, token, true);
        }
        private async Task<PlaylistOutput> GetPlayListItemsAsync(PlaylistInput input, CancellationToken token, bool paging)
        {
            JObject listJson;
            PlaylistOutput output = new PlaylistOutput();
            do
            {
                listJson = JObject.Parse(await GetJsonAsync(input, InputType.Playlist, token));
                output.TotalResults = int.Parse(listJson["pageInfo"]["totalResults"].ToString());
                input.PageToken = listJson["nextPageToken"].IsNullOrEmpty() != true ? listJson["nextPageToken"].ToString() : null;
                output.NextPage = input.PageToken;
                foreach (var item in listJson["items"])
                {
                    output.VideosData.Add(SetPlaylistContainsDetails(item["snippet"]));
                    output.TotalResults++;
                }
            } while (!listJson["nextPageToken"].IsNullOrEmpty() && !paging);
            return output;
        }
        private async Task<TrendingOutput> GetTrendingItemsAsync(TrendingInput input, CancellationToken token, bool paging)
        {
            JObject listJson;
            TrendingOutput output = new TrendingOutput();
            do
            {
                listJson = JObject.Parse(await GetJsonAsync(input, InputType.Trending, token));
                output.TotalResults = int.Parse(listJson["pageInfo"]["totalResults"].ToString());
                input.PageToken = listJson["nextPageToken"].IsNullOrEmpty() != true ? listJson["nextPageToken"].ToString() : null;
                output.NextPage = input.PageToken;
                foreach (var item in listJson["items"])
                {
                    output.VideosData.Add(SetTrendingContainsDetails(item));
                    output.TotalResults++;
                }
            } while (!listJson["nextPageToken"].IsNullOrEmpty() && !paging);

            return output;
        }
        private async Task<string> GetJsonAsync(object input, InputType type, CancellationToken token)
        {
            using (_httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(_baseUri);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Uri requrestUri;
                switch (type)
                {
                    case InputType.Playlist:
                        {
                            requrestUri = CreatePlaylistClientUri((PlaylistInput)input);
                            break;
                        }
                    case InputType.Trending:
                        {
                            requrestUri = CreateTredingClientUri((TrendingInput)input);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                using (var response = await _httpClient.GetAsync(requrestUri, token))
                {
                    using (var content = response.Content)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return await content.ReadAsStringAsync();
                        }
                        throw new GeneralExceptions().HandlePlaylistExceptions((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                    }
                }
            }
        }
        private Uri CreatePlaylistClientUri(PlaylistInput input)
        {
            if (string.IsNullOrEmpty(_apiKey)) throw new ArgumentNullException(nameof(_apiKey), "Api Key can not be null");
            if (string.IsNullOrEmpty(input.PlaylistId)) throw new ArgumentNullException(nameof(input.PlaylistId), "PlaylistId can not be null");
            UriBuilder builder = new UriBuilder(_baseUri + EntryPoints.Playlist)
            {
                Query = $"key={_apiKey}&" +
                        "part=snippet&" +
                        $"playlistId={input.PlaylistId}&" +
                        $"maxResults={input.MaxResultPerPage.SetMaxResultPerPage()}&" +
                        $"pageToken={input.PageToken}"
            };
            return builder.Uri;
        }
        private Uri CreateTredingClientUri(TrendingInput input)
        {
            if (string.IsNullOrEmpty(_apiKey)) throw new ArgumentNullException(nameof(_apiKey), "Api Key can not be null");
            if (input.Category == default(int)) throw new ArgumentNullException(nameof(input.Category), "Category can not be null");
            UriBuilder builder = new UriBuilder(_baseUri + EntryPoints.Videos)
            {
                Query = $"key={_apiKey}&" +
                        "part=snippet&" +
                        "chart=mostPopular&" +
                        $"maxResults={input.MaxResultPerPage.SetMaxResultPerPage()}&" +
                        $"pageToken={input.PageToken}&" +
                        $"regionCode={new TrendingEnums().GetCodeOfRegion(input.Region)}&" +
                        $"videoCategoryId={(int)input.Category}"
            };
            return builder.Uri;
        }
        private List<Thumbnails> SetThumbnails(JToken jsonData)
        {
            if (jsonData["thumbnails"].IsNullOrEmpty() != true)
            {
                List<Thumbnails> videoImages = new List<Thumbnails>();
                if (!jsonData["thumbnails"]["default"].IsNullOrEmpty())
                {
                    videoImages.Add(new Thumbnails
                    {
                        Kind = GeneralEnums.ImageThumbnails.Default,
                        ImageUri = new Uri(jsonData["thumbnails"]["default"]["url"].ToString()),
                        Height = int.Parse(jsonData["thumbnails"]["default"]["height"].ToString()),
                        Width = int.Parse(jsonData["thumbnails"]["default"]["width"].ToString())
                    });
                }
                if (!jsonData["thumbnails"]["medium"].IsNullOrEmpty())
                {
                    videoImages.Add(new Thumbnails
                    {
                        Kind = GeneralEnums.ImageThumbnails.Medium,
                        ImageUri = new Uri(jsonData["thumbnails"]["medium"]["url"].ToString()),
                        Height = int.Parse(jsonData["thumbnails"]["medium"]["height"].ToString()),
                        Width = int.Parse(jsonData["thumbnails"]["medium"]["width"].ToString())
                    });
                }
                if (!jsonData["thumbnails"]["high"].IsNullOrEmpty())
                {
                    videoImages.Add(new Thumbnails
                    {
                        Kind = GeneralEnums.ImageThumbnails.High,
                        ImageUri = new Uri(jsonData["thumbnails"]["high"]["url"].ToString()),
                        Height = int.Parse(jsonData["thumbnails"]["high"]["height"].ToString()),
                        Width = int.Parse(jsonData["thumbnails"]["high"]["width"].ToString())
                    });
                }
                if (!jsonData["thumbnails"]["standard"].IsNullOrEmpty())
                {
                    videoImages.Add(new Thumbnails
                    {
                        Kind = GeneralEnums.ImageThumbnails.Standard,
                        ImageUri = new Uri(jsonData["thumbnails"]["standard"]["url"].ToString()),
                        Height = int.Parse(jsonData["thumbnails"]["standard"]["height"].ToString()),
                        Width = int.Parse(jsonData["thumbnails"]["standard"]["width"].ToString())
                    });
                }
                if (!jsonData["thumbnails"]["maxres"].IsNullOrEmpty())
                {
                    videoImages.Add(new Thumbnails
                    {
                        Kind = GeneralEnums.ImageThumbnails.MaxResolution,
                        ImageUri = new Uri(jsonData["thumbnails"]["maxres"]["url"].ToString()),
                        Height = int.Parse(jsonData["thumbnails"]["maxres"]["height"].ToString()),
                        Width = int.Parse(jsonData["thumbnails"]["maxres"]["width"].ToString())
                    });
                }
                return videoImages;
            }
            return null;
        }
        private PlaylistOutput.Videos SetPlaylistContainsDetails(JToken item)
        {
            PlaylistOutput.Videos videoData = new PlaylistOutput.Videos();
            videoData.Kind = item["resourceId"]["kind"].ToString().Split('#')[1];
            videoData.VideoId = item["resourceId"]["videoId"].ToString();
            videoData.Title = item["title"].ToString();
            videoData.Description = item["description"].ToString();
            videoData.Position = int.Parse(item["position"].ToString()) + 1;
            videoData.ChannelId = item["channelId"].ToString();
            videoData.ChannelTitle = item["channelTitle"].ToString();
            videoData.PublishedAt = DateTime.Parse(item["publishedAt"].ToString());
            videoData.ThumbnailsData = SetThumbnails(item);
            return videoData;
        }
        private TrendingOutput.Videos SetTrendingContainsDetails(JToken item)
        {
            TrendingOutput.Videos videoData = new TrendingOutput.Videos();
            videoData.Kind = item["kind"].ToString().Split('#')[1];
            videoData.VideoId = item["id"].ToString();
            videoData.Title = item["snippet"]["title"].ToString();
            videoData.Description = item["snippet"]["description"].ToString();
            videoData.ChannelId = item["snippet"]["channelId"].ToString();
            videoData.ChannelTitle = item["snippet"]["channelTitle"].ToString();
            videoData.PublishedAt = DateTime.Parse(item["snippet"]["publishedAt"].ToString());
            videoData.CategoryId = (int)item["snippet"]["categoryId"];
            videoData.Category = (TrendingEnums.Categories)videoData.CategoryId;
            videoData.ThumbnailsData = SetThumbnails(item["snippet"]);
            return videoData;
        }
        private enum InputType
        {
            Playlist, Trending
        }
    }
}
