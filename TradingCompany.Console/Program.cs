using AutoMapper;

namespace TradingCompany.Console
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging.Abstractions;
    using TradingCompany.DALEF.AutoMapper;
 

    internal class Program
    {

       // static readonly string CONNECTION_STRING = "Data Source = localhost,1433; Database=TradingCompany;User ID = sa; Password=MyStr0ng!Pass123;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        static void Main(string[] args)
        {

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<UserProfileMap>();
            configExpression.AddProfile<OrderMap>();
            configExpression.AddProfile<ProductMap>();
            configExpression.AddProfile<UserMap>();
            configExpression.AddProfile<RoleMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            IMapper mapper = mapperConfig.CreateMapper();

            var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)          
            .Build();

            string connectionString = config.GetConnectionString("DefaultConnection");


            new AppMenu.AppMenu(connectionString, mapper).Show();

        }
       
    }
}
