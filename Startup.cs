using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApi.Models;
using TodoApi.Interface;
using TodoApi.Repository;
using TodoApi.Utility;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        //string myDb2ConnectionString = _configuration.GetConnectionString("myDb2");

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()));

            // Added - uses IOptions<T> for your settings.
            services.AddOptions();

            services.AddControllers();

            // Added - Confirms that we have a home for our DataConnection
            services.Configure<DataConnection>(Configuration.GetSection("AppDbConnection"));

            //services.AddTransient<ITeamRepoService, ITeamService>();
            //services.AddTransient<ITeam, TeamRepository>();
            services.AddTransient<ITeam, TeamRepository>(_ => new TeamRepository("Server=tcp:saas-sql-server.database.windows.net,1433;Initial Catalog=SaasUsers;Persist Security Info=False;User ID=SaasDb;Password=ATat0128;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=300"));
            services.AddTransient<IUser, UserRepository>(_ => new UserRepository("Server=tcp:saas-sql-server.database.windows.net,1433;Initial Catalog=SaasUsers;Persist Security Info=False;User ID=SaasDb;Password=ATat0128;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=300"));

            services.AddScoped<InvalidTokenAttribute>();
            //Add filter to whole api
            services.AddControllers(options =>
                       {
                           options.Filters.Add<InvalidTokenAttribute>();
                       });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}