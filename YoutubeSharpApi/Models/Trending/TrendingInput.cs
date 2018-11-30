using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeSharpApi.Models.Trending
{
    public class TrendingInput
    {
        public TrendingEnums.Regions Region { get; set; }
        public TrendingEnums.Categories Category { get; set; }
        public string PageToken { get; set; }
        public int MaxResultPerPage { get; set; }

        public TrendingInput()
        {
            this.PageToken = null;
            this.MaxResultPerPage = 25;
        }
    }
}
