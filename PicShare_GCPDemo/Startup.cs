using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PicShare_GCPDemo.Models;
using MySql.Data.MySqlClient;

namespace PicShare_GCPDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<CloudStorageOptions>(
                Configuration.GetSection("GoogleCloudStorage"));
            services.AddDbContext<PicShareContext>(options =>
                options.UseMySql(BuildMySqlConnectionString()));
        }

        private string BuildMySqlConnectionString()
        {
            var connectionString = new MySqlConnectionStringBuilder(
                Configuration["CloudSql:ConnectionString"])
            {
                SslMode = MySqlSslMode.Required,
                CertificateFile = Configuration["CloudSql:CertificateFile"]
            };
            return connectionString.ConnectionString;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
