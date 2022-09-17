Youtube Sharp Api
===============

[![NuGet](https://img.shields.io/nuget/dt/YoutubeSharpApi.svg)](https://www.nuget.org/packages/YoutubeSharpApi)
[![NuGet](https://img.shields.io/nuget/v/YoutubeSharpApi.svg)](https://www.nuget.org/packages/YoutubeSharpApi)
[![license](https://img.shields.io/github/license/omansak/YoutubeSharpApi.svg)](LICENSE)
[![Join the chat at https://discord.gg/SERVhPp](https://user-images.githubusercontent.com/7288322/34429152-141689f8-ecb9-11e7-8003-b5a10a5fcb29.png)](https://discord.gg/SERVhPp)

A minimal .NET STANDART wrapper for the Youtube Data API v3. Designed to let devs easily 
fetch public data (Playlists info,Trending) from Youtube.
The reason of returning the decoded JSON response directly is that you only need to read the Google API doc 
to use this library.

## Supported Platforms
- NET ve .NET Core	            |   2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0
- .NET Framework  	            |   4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8
- Mono	                        |   5.4, 6.4
- Xamarin.iOS	                    |   10.14, 12.16
- Xamarin.Mac	                    |   3.8, 5.16
- Xamarin.Android	                |   8.0, 10.0
- Universal Windows Platform	    |   10.0.16299, TBD
- Unity	                        |   2018,

## Features
* Get Playlist Info All or Pagination 
* Get Trending Info All or Pagination 
* 90 Regions / 31 Youtube Category

## Requirements
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
