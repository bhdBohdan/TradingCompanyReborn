using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using TradingCompany.ASP.MVC.Common.MappingProfiles; // ensure this assembly is referenced
using TradingCompany.BLL.Concrete;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.AutoMapper;
using TradingCompany.DALEF.Concrete;

namespace TradingCompany.ASP.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddLog4Net("log4net.config");
            });

            // Register AutoMapper (scan the assembly that contains your profiles)
            builder.Services.AddAutoMapper(typeof(ProductMap).Assembly, typeof(TradingCompany.DALEF.AutoMapper.UserMap).Assembly);

            builder.Services.AddSingleton<IMapper>(sp =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.ConstructServicesUsing(sp.GetService);
                    cfg.AddMaps(typeof(ProductMap).Assembly, typeof(SelectUserListItemProfile).Assembly);
                }, loggerFactory);

                return config.CreateMapper();
            });

            string connStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

            // DAL and BL registrations (IMapper is available now)
            builder.Services.AddTransient<IProductDAL>(sp => new ProductDALEF(connStr, sp.GetRequiredService<IMapper>()))
                            .AddTransient<IUserDAL>(sp => new UserDALEF(connStr, sp.GetRequiredService<IMapper>()))
                            .AddTransient<IRoleDAL>(sp => new RoleDALEF(connStr, sp.GetRequiredService<IMapper>()))

                            .AddTransient<IProductManager, ProductManager>()
                             .AddTransient<IAuthManager, AuthManager>();


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
           {
               options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
               options.SlidingExpiration = true;
               options.AccessDeniedPath = "/Account/Forbidden/";
               options.LoginPath = "/Account/Login/";
           });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}