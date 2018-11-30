using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeSharpApi.Models
{
    public class Thumbnails
    {
        public GeneralEnums.ImageThumbnails Kind { get; set; }
        public Uri ImageUri { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
