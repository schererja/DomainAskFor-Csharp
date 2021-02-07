using DomainAskFor.API.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DomainAskFor.API
{
  public class Startup
  {
    private const string AllowedOrigins = "_allowedSpecificOrigins";
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddCors(options =>
      {
        options.AddPolicy(name: AllowedOrigins, builder =>
        {
          builder.WithOrigins("https://localhost:44386", "https://localhost:5001").AllowAnyHeader().AllowAnyMethod();
        });
      });
      services.AddHttpClient<ISynonymsController, SynonymsController>();

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
      });
      services.AddDistributedRedisCache(option =>
        {
          option.Configuration = "localhost:6379";
          option.InstanceName = "master";
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();
      app.UseCors(AllowedOrigins);

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
