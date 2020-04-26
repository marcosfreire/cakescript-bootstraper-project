#addin "nuget:http://nexus.zarp.tech/repository/nuget-hosted?package=SharpZipLib&version=1.2.0"
#addin "nuget:http://nexus.zarp.tech/repository/nuget-hosted?package=Cake.Compression&version=0.2.4"

Task("zip-artifacts").WithCriteria(projectInfo.CanUploadArtifactsToNexus) 
.Does(() =>
{
    var packName = $"{projectInfo.ProjectName}.{projectInfo.CurrentBuild.Version}.zip";
    var packPath = System.IO.Path.Combine(projectInfo.Paths.Artifact.ToString(), packName);

    ZipCompress(projectInfo.Paths.PublishTo, packPath, 6);

    FileWriteText("./latest.txt", projectInfo.CurrentBuild.SemVersion);
});