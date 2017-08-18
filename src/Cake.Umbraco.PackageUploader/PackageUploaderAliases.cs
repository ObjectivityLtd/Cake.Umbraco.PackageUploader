using Cake.Core;
using Cake.Core.Annotations;
using System;

namespace Cake.Umbraco.PackageUploader
{
    [CakeAliasCategory("PackageUploader")]
    public static class PackageUploaderAliases
    {
        [CakeMethodAlias]
        public static UmbracoHttpManager GetUmbracoManager(this ICakeContext context, string umbracoUrl, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(umbracoUrl))
            {
                throw new ArgumentNullException(nameof(umbracoUrl));
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var umbracoClient = new UmbracoInstance(umbracoUrl, username, password).Login();
            var umbracoHttpManager = new UmbracoHttpManager(umbracoClient, new HttpContentFactory());

            return umbracoHttpManager;
        }
    }
}
