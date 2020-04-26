public class SonarInfo{
    
    public string ScannerPath { get; private set; }

    public SonarInfo(ICakeContext context,BuildSystem buildSystem){

        ScannerPath  = buildSystem.Jenkins.IsRunningOnJenkins ?  context.EnvironmentVariable("SONAR") + @"\SonarScanner.MSBuild.exe" : "";
    }
}