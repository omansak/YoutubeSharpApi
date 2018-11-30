using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using YoutubeSharpApi.Models.Playlist;

namespace YoutubeSharpApi
{
    public static class Extensions
    {
        public static int SetMaxResultPerPage(this int maxResults)
        {
            if (maxResults > 50)
                return 50;
            if (maxResults < 0)
                return 5;
            return maxResults;
        }
        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == System.String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }
}
