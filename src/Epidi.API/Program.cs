using Epidi.API.Common;
using Epidi.API.Extensions;
using Epidi.API.Validation;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.ViewModel.User;
using Epidi.ModelValidation;
using Epidi.Repository.DBContext;
using Epidi.Service.Helpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Serilog;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);


var service = builder.Services;
var env = builder.Environment;
var configuration = builder.Configuration;
// Add services to the container.

service.AddControllers();
service.AddEndpointsApiExplorer();
service.AddSwaggerGen();
service.AddMvc(option => option.EnableEndpointRouting = false)
              .ConfigureApiBehaviorOptions(options =>
              {
                  options.InvalidModelStateResponseFactory = c =>
                  {
                      return new ValidationFailedResult(c.ModelState);
                  };
              })
              .AddNewtonsoftJson();


service.AddFluentValidationAutoValidation();//.AddFluentValidationClientsideAdapters();



service.AddMemoryCache();

service.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;

});

service.AddJWTAuthentication(configuration);

#region ConfigureDb
service.Configure<ConnectionSettings>(options =>
{
    options.ConnectionString = configuration.GetConnectionString("DefaultConnection");
});
#endregion

#region FluentValidation
//service.AddSingleton<IValidator<RegisterUserViewModel>, RegisterUserViewModelValidator>();
#endregion

#region AddDBContext
service.AddDbContext<EpidiDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
#endregion

#region HealthCheck
service.AddHealthChecks().AddSqlServer(configuration.GetConnectionString("DefaultConnection"));

#endregion

#region Swagger Configuration
//service.AddSwaggerDocument(config =>
//{
//    config.PostProcess = document =>
//    {
//        document.Info.Version = "v1";
//        document.Info.Title = "Epidi";
//        document.Info.Description = "A simple ASP.NET Core web API performing CRUD operation on Workflow";
//        document.Info.TermsOfService = "None";
//        document.Info.Contact = new NSwag.OpenApiContact
//        {
//            Name = "Epidi",
//            Email = "info@epidi.com",
//            Url = ""
//        };
//        document.Info.License = new NSwag.OpenApiLicense
//        {
//            Name = "Use under LICX",
//            Url = "https://epidi.com/license"
//        };
//    };
//});
#endregion


#region RegisterService
service.CreateDependencies();
#endregion

service.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
service.Configure<FileUploads>(builder.Configuration.GetSection("FileUpload"));
service.Configure<EmailConfigs>(builder.Configuration.GetSection("EmailConfig"));
service.Configure<StraalConfigModel>(builder.Configuration.GetSection("StraalConfiguration"));

service.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var logger = new LoggerConfiguration()
         .ReadFrom.Configuration(configuration)
         .CreateLogger();

builder.Host.UseSerilog(logger);

var app = builder.Build();

app.UseCors(x => x
	   .AllowAnyOrigin()
	   .AllowAnyMethod()
	   .AllowAnyHeader());

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"upload")),
//    RequestPath = new PathString("/upload")
//});

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
    options.RoutePrefix = string.Empty;
});
//app.UseSwaggerUi3();
app.UseCors("CorsPolicy");

app.ConfigureCustomExceptionMiddleware();

app.UseMvc();



#region Healthcheck
app.UseHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
#endregion

//app.UseSerilogRequestLogging();

app.Run();



