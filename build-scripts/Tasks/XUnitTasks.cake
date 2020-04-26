#tool "nuget:https://nexus.zarp.tech/repository/nuget-hosted?package=xunit.runner.console&version=2.3.1"
#tool "nuget:https://nexus.zarp.tech/repository/nuget-hosted?package=OpenCover&version=4.7.922"

Task("run-tests").Does(() =>
{
        OpenCover(tool =>
        {
            tool.XUnit2("./tests/**/bin/Release/*.Tests.dll",
                new XUnit2Settings {
                    ShadowCopy = false,
                    XmlReport = true,
                    OutputDirectory = projectInfo.Paths.OpencoverOutputDirectory.ToString()
            });
        },
        projectInfo.Paths.OpencoverResultFile,
        new OpenCoverSettings(){
            Register = "administrator",
            SkipAutoProps = true,
            MergeByHash = true,
            NoDefaultFilters = true
        }
        .WithFilter("+[Maxi.*]*")
        .WithFilter("-[Maxi.*]*Tests")
    );
});