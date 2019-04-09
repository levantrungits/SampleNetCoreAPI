using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessAccess.Repository;
using DataAccess.ConfigurationManage;
using DataAccess.DBContext;
using DataAccess.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SampleNetCoreAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Init Builder 1 -> Get ConnectionString
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var connectionStringConfig = builder.Build();

            // Init Builder 2 -> Get && Add Configs From Database
            var config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                // FROM: static class EntityFrameworkExtensions
                .AddEnvironmentVariables().AddEntityFrameworkConfig(option =>
                    option.UseMySQL(connectionStringConfig.GetConnectionString("MySqlConnection"))
                );

            // Set Configuration
            Configuration = config.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add ConnectionString
            var sqlConnectionString = Configuration.GetConnectionString("MySqlConnection");
            services.AddDbContext<SampleNetCoreAPIContext>(options =>
                options.UseMySQL(sqlConnectionString)
            );
            #endregion

            #region Add Configuration to dependency injection
            services.AddSingleton<IConfiguration>(Configuration);
            #endregion

            #region Add Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            #endregion

            #region Add AutoMapper
            ConfigAutoMapper(services);
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        // Init Config AutoMapper for Class by Class
        public void ConfigAutoMapper(IServiceCollection services)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, Blog>();

            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
        }
    }
}
