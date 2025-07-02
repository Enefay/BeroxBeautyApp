using Microsoft.AspNetCore.Builder;
using BeroxApp;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();

builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("BeroxApp.Web.csproj");
await builder.RunAbpModuleAsync<BeroxAppWebTestModule>(applicationName: "BeroxApp.Web" );

public partial class Program
{
}
