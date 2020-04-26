using System.Text.RegularExpressions;

public class BuildInfo
{
    public bool IsLocalBuild { get; private set; }
    public bool IsRunningOnJenkins{ get; private set; }
    public bool IsRunningOnWindows{ get; private set; }
    
    public string PublishProfile{get;private set;}
    public string Version { get; private set; }
    public string SemVersion { get;  private set; }
    public string BuildNumber { get;  private set; }
    public string Configuration {get;private set;}

    public string TimeStamp{get;set;}
    public string JobUrl{get;private set;}
        
    public BuildInfo(ICakeContext context,BuildSystem buildSystem)
    {
        IsLocalBuild = buildSystem.IsLocalBuild;
        IsRunningOnWindows = context.IsRunningOnWindows();
        IsRunningOnJenkins = buildSystem.Jenkins.IsRunningOnJenkins;
        
        Configuration = context.Argument("configuration", "Release");

        JobUrl = context.EnvironmentVariable("BUILD_URL") ?? "";
        TimeStamp = context.EnvironmentVariable("BUILD_TIMESTAMP") ?? "";
        
        SetCurrentBuildVersions(context,buildSystem);
        SetPublishProfile(context);
    }

    public void SetCurrentBuildVersions(ICakeContext context,BuildSystem buildSystem)
    {
        if(IsLocalBuild)
        {
            SetLocalBuildVersions();
        }
        else
        {
            SemVersion = GetSemVer(context);
            BuildNumber = buildSystem.Jenkins.Environment.Build.BuildNumber.ToString();
            Version = ComposeVersion(SemVersion, BuildNumber);
        }
    }

    private void SetLocalBuildVersions()
    {
        SemVersion = "1.0.0";
        BuildNumber = "0";
        Version = "0";        
    }

    private string GetAssemblyVersion(ICakeContext context)
    {
        var  assemblyInfoPath = context.GetFiles("./**/AssemblyInfo.cs").First().ToString();

        var assemblyVersion = string.Empty;
        
        if (context.IsRunningOnWindows())
        {
            assemblyVersion = ReadSolutionInfoVersion(context, assemblyInfoPath);
        }

        return assemblyVersion;
    }


    private string GetSemVer(ICakeContext context)
    {
        var version = GetAssemblyVersion(context);

        Regex parseEx =
            new Regex(@"^(?<major>\d+)" +
                @"(\.(?<minor>\d+))?" +
                @"(\.(?<patch>\d+))?" +
                @"(\.(?<build>\d+))?$",
                RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        var match = parseEx.Match(version);

        if (!match.Success)
            throw new ArgumentException("Invalid version.", "version");

        var major = int.Parse(match.Groups["major"].Value, System.Globalization.CultureInfo.InvariantCulture);

        var minorMatch = match.Groups["minor"];
        int minor = 0;

        if (minorMatch.Success)
        {
            minor = int.Parse(minorMatch.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        var patchMatch = match.Groups["patch"];
        int patch = 0;

        if (patchMatch.Success)
        {
            patch = int.Parse(patchMatch.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        var buildMatch = match.Groups["build"];
        int build = 0;

        if(buildMatch.Success)
        {
            build = int.Parse(buildMatch.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        return string.Format("{0}.{1}.{2}", major, minor, patch);
    }

    private string ComposeVersion(string version, string buildNumber)
    {
        return string.Join(".", version, buildNumber);
    }

    private string ReadSolutionInfoVersion(ICakeContext context, string assemblyInfoPath)
    {
        var solutionInfo = context.ParseAssemblyInfo(assemblyInfoPath);

        if (!string.IsNullOrEmpty(solutionInfo.AssemblyFileVersion))
        {
            return solutionInfo.AssemblyFileVersion;
        }
        
        throw new CakeException("Could not parse version.");
    }

    private void SetPublishProfile(ICakeContext context)
    {
        var publishProfile = context.Argument("profile","");        
        PublishProfile = string.IsNullOrWhiteSpace(publishProfile) ? "JENKINS-CI" : publishProfile;
    }
}