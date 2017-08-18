using System.Net.Http;

namespace Cake.Umbraco.PackageUploader
{
    public class UmbracoHttpClient : IHttpClient
    {
        private readonly string xsrfToken;

        private readonly string baseUrl;

        private readonly HttpClient httpClient;

        public UmbracoHttpClient(HttpClient httpClient, string xsrf, string baseUrl)
        {
            this.httpClient = httpClient;

            this.xsrfToken = xsrf;

            this.baseUrl = baseUrl;
        }

        public HttpResponseMessage Post(string actionPath, HttpContent content)
        {
            var url = this.baseUrl + actionPath;

            content.Headers.Add("X-XSRF-TOKEN", this.xsrfToken);

            var response = httpClient.PostAsync(url, content).Result;

            return response;
        }
    }
}