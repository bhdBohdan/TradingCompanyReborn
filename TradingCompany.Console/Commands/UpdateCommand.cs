using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.Console.Interfaces
{
    using System;
    using TradingCompany.Console.Commands.Interfaces;

    public class UpdateCommand<T> : ICommand where T : class
    {
        private readonly IGenericDAL<T> _dal;
        private readonly Func<T> _factory; //anonymous function

        public UpdateCommand(IGenericDAL<T> dal)
        {
            _dal = dal;          
        }

        public string Description => $"Update a {typeof(T).Name}";

        public void Execute()
        {
            var instance = Activator.CreateInstance<T>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanWrite)
                {               
                    if (
                        prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        continue;
                    }
                    Console.Write($"Enter {prop.Name}: ");
                    var input = Console.ReadLine();
                    var convertedValue = Convert.ChangeType(input, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    prop.SetValue(instance, convertedValue);
                }
            }
           
            _dal.Update(instance);
            Console.WriteLine($"{typeof(T).Name} Updated successfully!");
        }
    }
}
