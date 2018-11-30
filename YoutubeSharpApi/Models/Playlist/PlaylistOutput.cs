using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeSharpApi.Models.Playlist
{
    public class PlaylistOutput
    {
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
    }
}
