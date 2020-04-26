#load "./GitInfo.cake";
#load "./BuildInfo.cake";
#load "./SonarInfo.cake";
#load "./PathsInfo.cake";
#addin "nuget:http://nexus.zarp.tech/repository/nuget-hosted?package=Newtonsoft.Json&version=11.0.2"

public class ProjectInfo
{
    public string Target { get; private set; }
    public string ProjectName { get; private set; }
    public string SolutionName { get; private set; }
    
    public BuildInfo CurrentBuild{get;set;}
    public GitInfo GitLab{get;private set;}

    [Newtonsoft.Json.JsonIgnore]
    public SonarInfo Sonar{get;private set;}

    [Newtonsoft.Json.JsonIgnore]
    public PathsInfo Paths{get;private set;}
       
    [Newtonsoft.Json.JsonIgnore]   
    public bool CanUploadArtifactsToNexus
    {
        get
        {
            if(CurrentBuild == null || GitLab == null) return false;

            return CurrentBuild.IsRunningOnJenkins && ( GitLab.IsMasterBranch || GitLab.IsReleaseBranch );
        }
    }

    public ProjectInfo(ICakeContext context,BuildSystem buildSystem , string solutionName)
    {
        ValidateParameters(context,buildSystem , solutionName);

        SolutionName = solutionName;
        
        Target = context.Argument("target", "Default");
        ProjectName = buildSystem.AppVeyor.Environment.Project.Name;
        
        GitLab = new GitInfo(buildSystem);
        Sonar = new SonarInfo(context,buildSystem);
        Paths = new PathsInfo(context,buildSystem);
        CurrentBuild = new BuildInfo(context,buildSystem);
    }

    private void ValidateParameters(ICakeContext context,BuildSystem buildSystem , string solutionName)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        if (buildSystem == null)
        {
            throw new ArgumentNullException("buildSystem");
        }

        if(string.IsNullOrWhiteSpace(solutionName))
        {
            throw new ArgumentException("solutionName");
        }

        if (!context.IsRunningOnWindows())
        {
            throw new NotImplementedException("Only on Windows.");
        }
    }    
}