const string SOLUTION_NAME = "MY_SOLUTION_NAME.sln";
private ProjectInfo projectInfo  = new ProjectInfo(Context,BuildSystem,SOLUTION_NAME);

Setup(context =>
{
  Information("Build starting...");
});

Teardown(context =>
{
    projectInfo.Paths.RemoveDirectories(Context);
});

#load "nuget:http://YOUR_NUGET_DNS/repository/nuget-hosted?package=YOUR_BUIL_SCRIPTS_PACKAGES"

Task("Default")
    .IsDependentOn("echo")
    .IsDependentOn("restore-packages")
    .IsDependentOn("sonar-begin")
    .IsDependentOn("build")
    .IsDependentOn("run-tests")
    .IsDependentOn("sonar-end")
    .IsDependentOn("zip-artifacts")
    .Does (() => {});

RunTarget(projectInfo.Target);
