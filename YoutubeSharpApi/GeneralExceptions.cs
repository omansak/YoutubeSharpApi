using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace YoutubeSharpApi
{
    public class GeneralExceptions
    {
        public Exception HandlePlaylistExceptions(int code, string content)
        {
            JObject obj = JObject.Parse(content);
            string exceptionMessage = string.Empty;
            int errorCode = int.Parse(obj["error"]["code"].ToString());
            foreach (var e in obj["error"]["errors"])
            {
                exceptionMessage += $"({e["reason"]}) {e["reason"]} : {e["message"]})";
            }
            switch (errorCode)
            {
                case 400:
                    return new BadRequest(exceptionMessage);
                case 403:
                    return new ForbiddenException(exceptionMessage);
                case 404:
                    return new NotFoundException(exceptionMessage);
                default:
                    return new Exception(exceptionMessage);
            }
        }

        private class NotFoundException : Exception
        {
           public NotFoundException(string message) : base(message) { }
        }
        private class ForbiddenException : Exception
        {
            public ForbiddenException(string message) : base(message) { }
        }
        private class BadRequest : Exception
        {
            public BadRequest(string message) : base(message) { }
        }
    }
}
