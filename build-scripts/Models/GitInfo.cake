public class GitInfo
{
    public string Branch{get;private set;}

    public bool IsMasterBranch { get; private set; }
    public bool IsReleaseBranch { get; private set; }
    public bool IsFeatureBranch { get; private set; }
    
    public string CommitId{get;private set;}
    public string CommitAuthor{get;private set;}

    public string RepoUrl {get;private set;}
    public string ActionType{get;private set;}    
    public string MergeRequestId{get;private set;}

    public GitInfo(ICakeContext context, BuildSystem buildSystem)
    {
        Branch = context.EnvironmentVariable("gitlabSourceBranch") ?? "";

        IsMasterBranch = Branch.Equals("master");
        IsReleaseBranch = Branch.Contains("release/");
        IsFeatureBranch = Branch.Contains("feature/");

        ActionType = context.EnvironmentVariable("gitlabActionType") ?? "";
        CommitId = context.EnvironmentVariable("GIT_COMMIT") ?? "";
        CommitAuthor = context.EnvironmentVariable("gitlabUserName") ?? "";
        RepoUrl = context.EnvironmentVariable("gitlabSourceRepoHomepage") ?? "";
        MergeRequestId = context.EnvironmentVariable("gitlabMergeRequestIid") ?? "";
    }
}