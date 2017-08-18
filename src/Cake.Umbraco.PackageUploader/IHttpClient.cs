using System.Net.Http;

namespace Cake.Umbraco.PackageUploader
{
    public interface IHttpClient
    {
        HttpResponseMessage Post(string actionPath, HttpContent content);
    }
}