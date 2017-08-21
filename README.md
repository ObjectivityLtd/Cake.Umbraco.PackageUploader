

# Cake.Umbraco.PackageUploader ![Build status](https://ci.appveyor.com/api/projects/status/github/ObjectivityLtd/Cake.Umbraco.PackageUploader?svg=true)
This is an extension for Cake framework. It allows automated installation of an Umbraco packages from local zip files.

# How to add Cake.UmbracoPackageUploader
In order to use it add the following line in your addin section:
```cake
#addin Cake.Umbraco.PackageUploader
```

# How to upload package to Umbraco instance
Code below shows how to define Cake task that uses this addin:

```cake
Task("InstallUmbracoPackage")
    .Does(() =>
{
    string umbracoRootUrl = "http://umbraco.local";
    string packagePath = @"C:\SomePackage.zip";
    var umbracoManager = GetUmbracoManager(umbracoUrl, "umbracoLogin", "umbracoPassword");
    umbracoManager.InstallPackage(packagePath);
});
```
