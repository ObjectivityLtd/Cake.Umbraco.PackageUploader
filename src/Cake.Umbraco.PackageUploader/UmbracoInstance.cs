using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Cake.Umbraco.PackageUploader
{
    public class UmbracoInstance
    {
        private const string postLoginPath = "/umbraco/backoffice/UmbracoApi/Authentication/PostLogin";

        private string InstanceUrl { get; }

        private string Username { get; }

        private string Password { get; }

        private string LoginUri
        {
            get
            {
                return this.InstanceUrl + postLoginPath;
            }
        }

        public UmbracoInstance(string instanceUrl, string username, string password)
        {
            this.InstanceUrl = instanceUrl;
            this.Username = username;
            this.Password = password;
        }

        public UmbracoHttpClient Login()
        {
            var client = new HttpClient();

            var jsonContent = JsonConvert.SerializeObject(new { username = this.Username, password = this.Password });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var result = client.PostAsync(this.LoginUri, content).Result;

            var xsrf = this.GetXsrfTokenFromCookie(result);

            var umbracoClient = new UmbracoHttpClient(client, xsrf, InstanceUrl);

            return umbracoClient;
        }

        private string GetXsrfTokenFromCookie(HttpResponseMessage message)
        {
            var cookie = message.Headers.GetValues("Set-Cookie");

            var cookieArray = cookie as string[];

            var xsrfRecord = cookieArray[0];

            var xsrfValue = xsrfRecord.Split('=')[1].Split(';')[0];

            return xsrfValue;
        }
    }
}
