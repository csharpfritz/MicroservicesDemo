using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ConsignedComputers.Inventory.Models;
using Swashbuckle.SwaggerGen.Application;
using Swashbuckle.Swagger.Model;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Net.Http;
using Microphone.AspNet;
using Microphone.Consul;

namespace ConsignedComputers.Inventory
{
  public class Startup
  {

    public static readonly int ApiVersion = 1;

    private static readonly Lazy<string> Host = new Lazy<string>(() => RancherMetadata.GetHost());
    private static readonly Lazy<string> Port = new Lazy<string>(() => RancherMetadata.GetPort());

    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      // Add database
      services.AddEntityFrameworkNpgsql()
        .AddDbContext<Models.InventoryContext>(options =>
          options.UseNpgsql(Configuration.GetConnectionString("Default"))
        );

      services.AddSwaggerGen();
      services.ConfigureSwaggerGen(ConfigureSwaggerGen);

      services.AddMicrophone<ConsulProvider>();
      services.Configure<ConsulOptions>(o =>
      {
        o.Host = Startup.Host.Value;
      });

      // Add framework services.
      services.AddMvc();
      services.AddApiVersioning(o => o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(ApiVersion, 0));

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, Models.InventoryContext context)
    {

      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      var port = Startup.Port.Value;
      var host = Startup.Host.Value;
      Console.WriteLine($"Running on rancher host IP {host}");

      context.Database.Migrate();
      context.EnsureSeedData();

      app.UseMvcWithDefaultRoute()
        .UseMicrophone("InventoryService", "1.0", new Uri($"http://{host}:{port}"));

      app.UseSwagger()
        .UseSwaggerUi();

    }

    private void ConfigureSwaggerGen(SwaggerGenOptions options)
    {

      options.SingleApiVersion(new Info
      {
        Version = "v" + ApiVersion,
        Title = "ConsignedComputers Inventory",
        Description = "Access to query and maintain inventory",
        TermsOfService = "None",
        Contact = new Contact { Name = "Jeff Fritz", Email = "jeff@jeffreyfritz.com", Url = "http://jeffreyfritz.com" }
      });

      //Determine base path for the application.
      var basePath = PlatformServices.Default.Application.ApplicationBasePath;

      //Set the comments path for the swagger json and ui.
      options.IncludeXmlComments(Path.Combine(basePath, "ConsignedComputers.Inventory.xml"));


    }


  }

  public static class RancherMetadata

  {





    public static string GetHost()

    {

      var host = HttpGet("http://rancher-metadata/2015-12-19/self/host/agent_ip");
      return host;

    }



    public static string GetPort()
    {

      var port = HttpGet("http://rancher-metadata/2015-12-19/self/service/ports/0").Split(':')[0];
      return port;

    }



    private static string HttpGet(string uri)

    {

      var http = new HttpClient();

      http.BaseAddress = new Uri(uri);

      var res = http.GetStringAsync("").Result;

      return res;

    }

  }
}
