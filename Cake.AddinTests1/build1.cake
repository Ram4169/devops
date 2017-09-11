///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./DevOpsDemo.sln");
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
	Information("Hello Clean!");
});
Task("Restore")
.IsDependentOn("Clean")
.Does(() => {	
	Information("Hello Restore!");
});
Task("Build")
.IsDependentOn("Restore")
.Does(() => {	
	Information("Hello Build!");
});
Task("Run-Unit-Tests")
.IsDependentOn("Build")
.Does(() => {	
	Information("Hello Run-Unit-Tests!");
});
Task("Run-Integration-Test")
.IsDependentOn("Run-Unit-Tests")
.Does(() => {	
	Information("Hello Run integration test!");
});
Task("Generate-Code-Coverage")
.IsDependentOn("Run-Integration-Test")
.Does(() => {	
	Information("Hello Generate-Code-Coverage!");
});
Task("Generate-Artifacts")
.IsDependentOn("Generate-Code-Coverage")
.Does(() => {	
	Information("Hello Generate-Artifacts!");
});
Task("Default")
.IsDependentOn("Generate-Artifacts");
//.Does(() => {
//	Information("Hello Cake!");
//});
RunTarget(target);