public class GitInfo
{
    public string Branch{get;private set;}

    public bool IsPullRequest { get; private set; }
    public bool IsMasterBranch { get; private set; }
    public bool IsReleaseBranch { get; private set; }
    public bool IsFeatureBranch { get; private set; }
    
    public string CommitId{get;private set;}
    public string CommitAuthor{get;private set;}
    public string CommitMessage{get;private set;}
    public string ActionType{get;private set;}
    public string RepoUrl {get;private set;}
    public string Token{get;private set;}

    public GitInfo(BuildSystem buildSystem)
    {
        Branch = buildSystem.AppVeyor.Environment.Repository.Branch;

        IsMasterBranch = Branch.Equals("master");
        IsReleaseBranch = Branch.Contains("release/");
        IsFeatureBranch = Branch.Contains("feature/");

        IsPullRequest = buildSystem.IsPullRequest;

        CommitId = buildSystem.AppVeyor.Environment.Repository.Commit.Id;        
        CommitMessage = buildSystem.AppVeyor.Environment.Repository.Commit.Message;
        CommitAuthor = buildSystem.AppVeyor.Environment.Repository.Commit.Author;

        Token= buildSystem.GitLabCI.Environment.Build.Token;
        RepoUrl = buildSystem.GitLabCI.Environment.Build.RepoUrl;        
    }
}