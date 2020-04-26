#addin "nuget:http://nexus.zarp.tech/repository/nuget-hosted?package=Cake.Json&version=4.0.0"
#addin "nuget:http://nexus.zarp.tech/repository/nuget-hosted?package=Newtonsoft.Json&version=11.0.2"

Task("echo").Does(() =>
{
  Information(SerializeJson(projectInfo));
});