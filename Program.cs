using Microsoft.AspNetCore.HttpLogging;
using WebApi.Core;
using WebApi.Data;
using WebApi.Data.Repositories;
namespace WebApi;

public class Program {
   public static void Main(string[] args) {
      var builder = WebApplication.CreateBuilder(args);
      
      // Configure logging
      builder.Logging.ClearProviders();
      builder.Logging.AddConsole();
      builder.Logging.AddDebug();
      
      // Configure DI-Container -----------------------------------------
      // add http logging 
      builder.Services.AddHttpLogging(opts =>
         opts.LoggingFields = HttpLoggingFields.All);
      
      // Add controllers
      builder.Services.AddControllers();
      
      builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
      builder.Services.AddScoped<ICarsRepository, CarsRepository>();
      builder.Services.AddScoped<IDataContext, DataContext>();
      
      var app = builder.Build();

      // Configure the HTTP request pipeline.
      app.MapControllers();

      app.Run();
   }
}