
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var sourceDir = "..\\src";
var outputDir = "..\\bin";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");

    if(!DirectoryExists(outputDir))
    {
        Information("Output directory does not exist.");
        CreateDirectory(outputDir);
    }
    else
    {
        CleanDirectory(outputDir);
    }
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("BuildSolution")
    .Description("Builds Cake.Umbraco.PackageUploader solution")
    .Does(() =>
{
    var solution = sourceDir + "\\Cake.Umbraco.PackageUploader.sln";

    NuGetRestore(solution);

    var buildOutputDir = "\"" + MakeAbsolute(Directory(outputDir)).FullPath + "\"";
    Information(buildOutputDir);

    MSBuild(solution, settings => 
        settings.SetConfiguration("Release"));
});

Task("NuGet")
    .Description("Create nuget package")
    .Does(()=>
{
    var packagePath = outputDir;

    if(!DirectoryExists(packagePath))
    {
        CreateDirectory(packagePath);
    }

    var nuspecFile = sourceDir + "\\Cake.Umbraco.PackageUploader.nuspec";

    var nuGetPackSettings   = new NuGetPackSettings {
        BasePath        = sourceDir + "\\Cake.Umbraco.PackageUploader\\bin\\Release\\",
        OutputDirectory = packagePath
    };

    NuGetPack(nuspecFile, nuGetPackSettings);
});

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("This is the default task which will be ran if no specific target is passed in.")
    .IsDependentOn("BuildSolution")
    .IsDependentOn("NuGet");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
