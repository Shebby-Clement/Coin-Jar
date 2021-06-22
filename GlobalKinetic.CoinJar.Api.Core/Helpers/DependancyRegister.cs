using GlobalKinetic.CoinJar.Data;
using GlobalKinetic.CoinJar.Data.Implementations;
using GlobalKinetic.CoinJar.Data.Interfaces;
using GlobalKinetic.CoinJar.Data.Repositories.Implementation;
using GlobalKinetic.CoinJar.Data.Repositories.Interfaces;
using GlobalKinetic.CoinJar.Services.Implementations;
using GlobalKinetic.CoinJar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace GlobalKinetic.CoinJar.Api.Core.Helpers
{
    public class DependancyRegister
    {
        private static IConfiguration _config;
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            _config = configuration;

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // For Entity Framework  
            services.AddDbContext<CoinJarDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("ConnStr")));

            services.AddControllers();

            services.AddControllersWithViews();

            services.AddRazorPages();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            services.AddTransient<IDbFactory, DbFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            



            // Repositories

            ////services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<ICoinRepository, CoinRepository>();

            // Services

            services.AddTransient<ICoinService, CoinService>();

            // Swagger

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GlobalKinetic.CoinJar.Api", Version = "v1" });
            });
        }
    }
}
