#addin "nuget:http://nexus.zarp.tech/repository/nuget-hosted?package=Cake.FileHelpers&version=3.2.1"
#addin "nuget:http://nexus.zarp.tech/repository/nuget-hosted?package=Cake.Json&version=4.0.0"
#addin "nuget:http://nexus.zarp.tech/repository/nuget-hosted?package=Newtonsoft.Json&version=11.0.2"

Task("build-project").Does(() =>
{
    var  msBuildSettings = new MSBuildSettings()
                .SetConfiguration(projectInfo.CurrentBuild.Configuration)
                .SetMSBuildPlatform(MSBuildPlatform.Automatic)
                .UseToolVersion(MSBuildToolVersion.VS2017)
                .SetVerbosity(Verbosity.Normal)
                .SetPlatformTarget(PlatformTarget.MSIL);

    var settings = msBuildSettings
                .WithProperty("DeployOnBuild", "true")
                .WithProperty("PublishProfile", projectInfo.CurrentBuild.PublishProfile)
                .WithProperty("publishUrl", projectInfo.Paths.PublishTo.ToString());

    MSBuild(projectInfo.SolutionName, settings);

    GenerateBuildInfo(projectInfo);
});

private void GenerateBuildInfo(ProjectInfo projectInfo)
{
    var buildInfoFile = projectInfo.Paths.PublishTo.ToString() + "/build-version.json";   
    FileWriteText(buildInfoFile, SerializeJson(projectInfo));
}