public class PathsInfo
{
     public DirectoryPath Src{get;private set;} 
     public DirectoryPath PublishTo{get;private set;}
     
     public DirectoryPath Artifact{get;private set;} 
     public DirectoryPath TestResults{get;private set;}      
     public DirectoryPathCollection ToClean { get; private set; }

     public FilePath OpencoverResultFile{get;private set;}
     public DirectoryPath OpencoverOutputDirectory{get;private set;}

     public string SonarCoverageExclusionFiles{get;private set;}

     public PathsInfo(ICakeContext context,BuildSystem buildSystem)
     {
          PublishTo = context.MakeAbsolute((DirectoryPath)(context.Directory("./publish")));
          Artifact = context.MakeAbsolute((DirectoryPath)(context.Directory("./artifact")));
          TestResults = context.MakeAbsolute((DirectoryPath)(context.Directory("./test-results")));
          OpencoverOutputDirectory = context.MakeAbsolute((DirectoryPath)(context.Directory("./opencover-results")));          
          OpencoverResultFile = OpencoverOutputDirectory.CombineWithFilePath("result.xml");
          
          ConfigureDirectoriesToClean(context);
          EnsureDirectoryExists(context);

          SetSonarqubeCoverageExclusions(context);
     }

     public void RemoveDirectories(ICakeContext context)
     {
        var recursive = true;        
        context.DeleteDirectory(PublishTo,recursive);        
        context.DeleteDirectory(Artifact,recursive);
        context.DeleteDirectory(OpencoverOutputDirectory,recursive);
     }

     private void ConfigureDirectoriesToClean(ICakeContext context)
     {
        ToClean = new DirectoryPathCollection(new[] {
                    PublishTo,
                    Artifact,
                    TestResults,
                    OpencoverOutputDirectory
                }, PathComparer.Default);

        ToClean.Add(context.GetDirectories("./**/bin"));
        ToClean.Add(context.GetDirectories("./**/packages"));
        ToClean.Add(context.GetDirectories("./**/obj"));
        ToClean.Add(context.GetDirectories("./**/obj"));
        ToClean.Add(context.GetDirectories("./**/.sonarqube"));                
    }

    private void EnsureDirectoryExists(ICakeContext context)
    {
        context.EnsureDirectoryExists(PublishTo);
        context.EnsureDirectoryExists(Artifact);
        context.EnsureDirectoryExists(OpencoverOutputDirectory);
    }

    private void SetSonarqubeCoverageExclusions(ICakeContext context)
    {
        const string SONAR_COVERAGE_EXCLUSION_PATH = "./.sonar.coverage.exclusions";

        var addSonarCoverageExclusions = context.FileExists(SONAR_COVERAGE_EXCLUSION_PATH);

        if(!addSonarCoverageExclusions) return ;

        var exclusions = new StringBuilder();

        context.Information("SONAR COVERAGE EXCLUSION FILES ...");

        foreach(var exclusion in context.FileReadLines(SONAR_COVERAGE_EXCLUSION_PATH))
        {
            context.Information("added for sonar coverage exclusion: " + exclusion.Trim());
            exclusions.AppendFormat("{0},", exclusion.Trim());
        }

        SonarCoverageExclusionFiles = exclusions.ToString(0, exclusions.Length - 1);
    }
}