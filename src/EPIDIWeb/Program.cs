using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Repository.DBContext;
using Epidi.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


var service = builder.Services;
var env = builder.Environment;
var configuration = builder.Configuration;

// Add services to the container.
service.AddControllersWithViews().AddRazorRuntimeCompilation();
service.AddControllersWithViews();
service.AddApplicationInsightsTelemetry();
service.AddSession();

#region ConfigureDb
service.Configure<ConnectionSettings>(options =>
{
    options.ConnectionString = configuration.GetConnectionString("DefaultConnection");
});
#endregion

#region AddDBContext
service.AddDbContext<EpidiDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
#endregion


#region RegisterService
service.CreateDependencies();
#endregion


service.Configure<StraalConfigModel>(builder.Configuration.GetSection("StraalConfiguration"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"PdfFile")),
    RequestPath = new PathString("/PdfFile")
});
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();