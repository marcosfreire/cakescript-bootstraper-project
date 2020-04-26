Task("clean").Does(() =>
{
    CleanDirectories(projectInfo.Paths.ToClean);
    if(FileExists("./latest.txt")) DeleteFile("./latest.txt");
});

Task("restore-packages").IsDependentOn("clean").Does(() =>
{
       NuGetRestore(projectInfo.SolutionName, new NuGetRestoreSettings {NoCache = true});
});