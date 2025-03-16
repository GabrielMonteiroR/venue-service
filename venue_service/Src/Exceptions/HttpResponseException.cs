using System.Net;

namespace venue_service.Src.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }
        public string Title { get; }
        public string Details { get; }
        public DateTime Timestamp { get; }

        public HttpResponseException(HttpStatusCode statusCode, string title, string message, string details = null)
            : base(message)
        {
            StatusCode = (int)statusCode;
            Title = title;
            Details = details;
            Timestamp = DateTime.UtcNow;
        }
    }
}
