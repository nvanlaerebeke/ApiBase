using System;
using System.Linq;

namespace ApiBase.Controller.Response
{
    public static class StringExtensions
    {
        public static string CombineAsURL(this string baseURL, params string[] segments)
        {
            return AppendToURL(baseURL, segments);
        }

        public static string AppendToURL(this string baseURL, params string[] segments)
        {
            {
                return string.Join("/", new[] { baseURL.TrimEnd('/') }.Concat(segments.Select(s => s.Trim('/'))));
            }
        }
    }

    public static class UriExtensions
    {
        public static Uri Append(this Uri uri, params string[] segments)
        {
            return new Uri(Append_ToAbsoluteUriString(uri, segments));
        }

        public static string Append_ToAbsoluteUriString(this Uri uri, string[] segments)
        {
            return uri.AbsoluteUri.AppendToURL(segments);
        }
    }
}
