#tool "nuget:?package=xunit.runner.console"
#tool nuget:?package=docfx.console&version=2.18.5
#addin nuget:?package=Cake.DocFx
#addin nuget:?package=Cake.Docker
#tool nuget:?package=Codecov
#addin nuget:?package=Cake.Codecov
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////
var solutionPath = File("./DevOpsDemo.sln");
var solution = ParseSolution(solutionPath);
var projects = solution.Projects;
var projectPaths = projects.Select(p => p.Path.GetDirectory());
var artifacts = "./dist/";
var publishingError = false;

//BuildParameters parameters = BuildParameters.GetParameters(Context, BuildSystem);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {	
	// Clean solution directories.
    foreach(var path in projectPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin/" + configuration);
        CleanDirectories(path + "/**/obj/" + configuration);
    }

    Information("Cleaning common files...");
    CleanDirectory(artifacts);
});
Task("Restore")
.IsDependentOn("Clean")
.Does(() => {	
	// Restore all NuGet packages.
    Information("Restoring solution...");
    NuGetRestore(solutionPath);
});
Task("Build")
.IsDependentOn("Restore")
.Does(() => {	
	Information("Building solution...");
    MSBuild(solutionPath, settings =>
        settings.SetPlatformTarget(PlatformTarget.MSIL)
            .SetMSBuildPlatform(MSBuildPlatform.x86)
            .UseToolVersion(MSBuildToolVersion.VS2017)
            .WithProperty("TreatWarningsAsErrors","true")
            .SetVerbosity(Verbosity.Quiet)
            .WithTarget("Build")
            .SetConfiguration(configuration));
});
Task("Run-Unit-Tests")
.IsDependentOn("Build")
.Does(() => {	
	//Information("Running unit tests...");
	//MSTest("./UnitTestDevOps/bin/Debug/UnitTestDevOps.dll");
	MSTest("./UnitTestDevOps/bin/Debug/UnitTestDevOps.dll", new MSTestSettings() {ToolPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\MSTest.exe" });
});
Task("Run-Integration-Test")
.IsDependentOn("Run-Unit-Tests")
.Does(() => {	
		MSTest("./IntegrationTesting/bin/Debug/IntegrationTesting.dll", new MSTestSettings() {ToolPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\MSTest.exe" });
});
Task("Generate-Code-Coverage")
.IsDependentOn("Run-Integration-Test")
.Does(() => {	
	Codecov("coverage.xml");
});
Task("Generate-Artifacts")
.IsDependentOn("Generate-Code-Coverage")
.Does(() => {
	Zip("./docfx/_site/", artifacts + "/docfx.zip");
});
Task("Default")
.IsDependentOn("Generate-Artifacts");

RunTarget(target);