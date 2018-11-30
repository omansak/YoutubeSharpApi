using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeSharpApi.Models
{
    public class StatusEnum
    {
        public enum HttpCodes
        {
            Success,
            NotFound,
            Forbidden,
            Error
        }
    }
}
