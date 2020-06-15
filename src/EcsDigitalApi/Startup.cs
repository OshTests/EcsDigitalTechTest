using AutoMapper;
using EcsDigitalApi.Data;
using EcsDigitalApi.Repositories;
using EcsDigitalApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EcsDigitalApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:63855")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddControllers();
            services.AddScoped<IMapper, Mapper>(serviceProvider => new Mapper(new MapperConfiguration(Mappers.CreateMaps)));
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IMakerService, MakerService>();
            services.AddScoped<IRelatedWordsService, RelatedWordsService>();
            services.AddScoped<IRelatedWordsService, RelatedWordsService>();

            //Using the MVC built in dependency injection (scoped by default).
            services.AddDbContext<CarsContext>(options => options.UseInMemoryDatabase("CarsContext"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<CarsContext>();
                context.SeedInMemory().Wait();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
