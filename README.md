Youtube Sharp Api
===============

A minimal .NET STANDART wrapper for the Youtube Data API v3. Designed to let devs easily 
fetch public data (Playlists info,Trending) from Youtube.
The reason of returning the decoded JSON response directly is that you only need to read the Google API doc 
to use this library.

## Features
* Get Playlist Info All or Pagination 
* Get Trending Info All or Pagination 
* 90 Regions / 31 Youtube Category

## Requirements
* .NET CORE >= 2.1
* Newtonsoft Json Library

## Usage 
```c#
          var youtubeClient = new YoutubeClient("-- your api key --");

          var response = await youtubeClient.GetPlayListAsync(
                                              new PlaylistInput{
                                              PlaylistId = "-- playlist url --"}, CancellationToken.None));
          var response = await youtubeClient.GetTrendingAsync(
                                              new TrendingInput{
                                              Category = TrendingEnums.Categories.Music,
                                              Region = TrendingEnums.Regions.Turkey}), CancellationToken.None);
```
With Paging
```c#
        var youtubeClient = new YoutubeClient("-- your api key --");

        var trendingInput = new TrendingInput()
        {
        MaxResultPerPage = 20,
        Category = TrendingEnums.Categories.Music,
        Region = TrendingEnums.Regions.Turkey
        };

        var response = await youtubeClient.GetTrendingWithPagingAsync(trendingInput, token: new CancellationToken());
        playlistInput.PageToken = responsePlaylistPaging.NextPage; // important to next page

```
Output
```c#
        public string Kind { get; set; }
        public string NextPage { get; set; }
        public int TotalResults { get; set; }
        public List<Videos> VideosData { get; set; }
        public class Videos
        {
            public string Title { get; set; }
            public string VideoId { get; set; }
            public int Position { get; set; }
            public string Description { get; set; }
            public DateTime PublishedAt { get; set; }
            public string ChannelId { get; set; }
            public string ChannelTitle { get; set; }
            public string Kind { get; set; }
            public List<Thumbnails> ThumbnailsData { get; set; }
        }

        public PlaylistOutput()
        {
            this.VideosData = new List<Videos>();
        }
```

## Youtube Data API v3
- [Youtube Data API v3 Doc](https://developers.google.com/youtube/v3/)
- [Obtain API key from Google API Console](http://code.google.com/apis/console)

## Contact

For bugs, complain and suggestions please [file an Issue here](https://github.com/omansak/YoutubeSharpApi/issues) 
