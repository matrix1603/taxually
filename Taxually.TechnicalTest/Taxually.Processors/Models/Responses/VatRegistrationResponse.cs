using System.Net;

namespace Taxually.Processors.Models.Responses
{
    public class VatRegistrationResponse 
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
