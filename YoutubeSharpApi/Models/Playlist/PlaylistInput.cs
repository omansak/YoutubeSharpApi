using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeSharpApi.Models.Playlist
{
    public class PlaylistInput
    {
        // * : Required
        public string PlaylistId { get; set; } // *
        public string PageToken { get; set; }
        public int MaxResultPerPage { get; set; }

        public PlaylistInput()
        {
            PageToken = null;
            this.MaxResultPerPage = 25;
        }
    }
}
