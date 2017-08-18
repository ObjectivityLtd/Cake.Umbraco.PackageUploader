using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Cake.Umbraco.PackageUploader
{
    public class UmbracoHttpManager
    {
        private readonly IHttpContentFactory httpContentFactory;

        private readonly IHttpClient httpClient;

        public UmbracoHttpManager(IHttpClient client, IHttpContentFactory factory)
        {
            this.httpClient = client;
            this.httpContentFactory = factory;
        }

        public void InstallPackage(string fileName)
        {
            var result = this.PerformStep("UploadLocalPackage", this.httpContentFactory.FileUploadContent(fileName));
            result = this.PerformStep("Import", this.httpContentFactory.ObjectContent(result));
            result = this.PerformStep("InstallFiles", this.httpContentFactory.ObjectContent(result));
            result = this.PerformStep("CheckRestart", this.httpContentFactory.ObjectContent(result));
            result = this.PerformStep("InstallData", this.httpContentFactory.ObjectContent(result));
            result = this.PerformStep("CleanUp", this.httpContentFactory.ObjectContent(result));
        }

        private object PerformStep(string actionName, HttpContent content)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException(nameof(actionName));
            }

            var actionPath = "/umbraco/backoffice/UmbracoApi/PackageInstall/" + actionName;

            var response = this.httpClient.Post(actionPath, content);

            var responseAsJson = this.GetJsonResponse(response);

            return responseAsJson;
        }
     
        private object GetJsonResponse(HttpResponseMessage response)
        {
            var responseContent = response.Content.ReadAsStringAsync().Result;

            //ommit some strange characters at the content begining
            responseContent = responseContent.Substring(responseContent.IndexOf('{'));

            return JsonConvert.DeserializeObject(responseContent);
        }
    }
}
