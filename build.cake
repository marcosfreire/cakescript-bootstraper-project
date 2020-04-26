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

#load "./build-scripts/Tasks/EchoTask.cake";
#load "./build-scripts/Models/ProjectInfo.cake";
#load "./build-scripts/Tasks/NugetTasks.cake";
#load "./build-scripts/Tasks/SonarTasks.cake";
#load "./build-scripts/Tasks/BuildTask.cake";
#load "./build-scripts/Tasks/XUnitTasks.cake";
#load "./build-scripts/Tasks/ZipTask.cake";

Task("Default")
    .IsDependentOn("echo")
    //.IsDependentOn("restore-packages")
    .IsDependentOn("sonar-begin")
    // .IsDependentOn("build")
    // .IsDependentOn("run-tests")
    // .IsDependentOn("sonar-end")
    // .IsDependentOn("zip-artifacts")
    .Does (() => {});

RunTarget(projectInfo.Target);