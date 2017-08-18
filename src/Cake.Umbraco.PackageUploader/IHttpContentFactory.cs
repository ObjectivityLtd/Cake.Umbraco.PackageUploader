using System.Net.Http;

namespace Cake.Umbraco.PackageUploader
{
    public interface IHttpContentFactory
    {
        HttpContent FileUploadContent(string fileName);

        StringContent ObjectContent(object objectToSend);
    }
}