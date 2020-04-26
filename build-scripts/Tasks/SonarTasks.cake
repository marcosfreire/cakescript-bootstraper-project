Task("sonar-begin").WithCriteria(projectInfo.CurrentBuild.IsRunningOnJenkins)
.Does(() =>
{
    var arguments = new ProcessArgumentBuilder()
        .Append("begin")
        .Append($"/key:{projectInfo.ProjectName} /name:{projectInfo.ProjectName} /version:{projectInfo.CurrentBuild.Version}")        
        .Append($"/d:sonar.analysis.projeto=\"{projectInfo.ProjectName}\"")
        .Append($"/d:sonar.analysis.user=\"{projectInfo.GitLab.CommitAuthor}\"")
        //.Append($"/d:sonar.cs.opencover.reportsPaths={projectInfo.Paths.OpencoverOutputDirectory}")
        .Append($"/d:sonar.cs.xunit.reportsPaths={projectInfo.Paths.OpencoverOutputDirectory}");

    AddSonarCoverageExclusionFiles(arguments,projectInfo);

    StartProcess(projectInfo.Sonar.ScannerPath.ToString(), new ProcessSettings{ Arguments = arguments });
});

Task("sonar-end").WithCriteria(projectInfo.CurrentBuild.IsRunningOnJenkins).Does(() =>
{
    var arguments = new ProcessArgumentBuilder().Append("end");
    StartProcess(projectInfo.Sonar.ScannerPath.ToString(), new ProcessSettings{ Arguments = arguments });
});

private void AddSonarCoverageExclusionFiles(ProcessArgumentBuilder arguments , ProjectInfo projectInfo)
{
    if(!string.IsNullOrWhiteSpace(projectInfo.Paths.SonarCoverageExclusionFiles))
    {
        arguments = arguments.Append($"/d:sonar.coverage.exclusions={projectInfo.Paths.SonarCoverageExclusionFiles}");
    }
}