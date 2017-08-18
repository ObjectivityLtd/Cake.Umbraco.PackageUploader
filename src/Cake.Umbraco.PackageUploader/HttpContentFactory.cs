using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Cake.Umbraco.PackageUploader
{
    public class HttpContentFactory : IHttpContentFactory
    {
        public HttpContent FileUploadContent(string fileName)
        {
            var fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException();
            }

            var fileStream = new FileStream(fileInfo.FullName, FileMode.Open);
            var streamContent = new StreamContent(fileStream);

            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = fileInfo.Name
            };

            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-zip-compressed");

            var multiPartContent = new MultipartFormDataContent
            {
                streamContent
            };

            return multiPartContent;
        }

        public StringContent ObjectContent(object objectToSend)
        {
            if (objectToSend == null)
            {
                throw new ArgumentNullException(nameof(objectToSend));
            }

            var serializedObject = JsonConvert.SerializeObject(objectToSend);

            var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            return content;
        }
    }
}
