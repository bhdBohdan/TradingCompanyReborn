using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using TradingCompany.BLL.Concrete;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Automapper;
using TradingCompany.DALEF.AutoMapper;
using TradingCompany.DALEF.Concrete;
using TradingCompany.WPF2.ViewModels;
using TradingCompany.WPF2.Windows;

namespace TradingCompany.WPF2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Services = BuildServiceProvider();

            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var login = Services.GetRequiredService<Login>();
            bool result = login.ShowDialog() ?? false;
            if (result)
            {
                Current.MainWindow = Services.GetRequiredService<HomePage>();
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow.Show();
            }
            else
            {
                Current.Shutdown();
            }
        }

        private static IServiceProvider BuildServiceProvider()
        {
            // Configure DI container
            var services = new ServiceCollection();

            // Logging
            services.AddLogging(builder =>
            {
                builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information);
            });

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<IMapper>(sp =>
            {
                // Ensure logger factory is available for MapperConfiguration constructor (some AutoMapper setups expect it)
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

                var config = new MapperConfiguration(cfg =>
                {
                    // Allow AutoMapper to construct profile instances from DI so profiles can receive ILoggerFactory / ILogger<T>
                    cfg.ConstructServicesUsing(sp.GetService);

                    // Load profiles from the DAL EF mapper assembly
                    cfg.AddMaps(typeof(UserMap).Assembly);
                    cfg.AddMaps(typeof(RoleMap).Assembly);
                    cfg.AddMaps(typeof(UserProfileMap).Assembly);
                }, loggerFactory);

                // Create IMapper that will use the service provider to resolve services when mapping
                return config.CreateMapper();
            });

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            string connStr = configuration.GetConnectionString("DefaultConnection") ?? "";

            // DAL and BL registrations
            services.AddTransient<IProductDAL>(sp => new ProductDALEF(connStr, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IUserDAL>(sp => new UserDALEF(connStr, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IUserProfileDAL>(sp => new UserProfileDALEF(connStr, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IRoleDAL>(sp => new RoleDALEF(connStr, sp.GetRequiredService<IMapper>()));

           // services.AddTransient<IMovieManager, MovieManager>();
            services.AddTransient<IAuthManager, AuthManager>();
            services.AddTransient<IProfileManager, ProfileManager>();
            services.AddTransient<IRoleManager, RoleManager>();

            // Register windows so they can be resolved with DI
            //services.AddTransient<MovieListViewModel>();
            services.AddTransient<LoginViewModel>();
	        services.AddTransient<RegisterViewModel>();
            services.AddTransient<UserProfileViewModel>();
            //services.AddTransient<MovieDetailsSimpleViewModel>();


            services.AddTransient<HomePage>();
            services.AddTransient<Login>();
            services.AddTransient<Register>();
            services.AddTransient<UserProfile>();
            //services.AddTransient<ProfilePage>();

            return services.BuildServiceProvider();
        }

    }

}
