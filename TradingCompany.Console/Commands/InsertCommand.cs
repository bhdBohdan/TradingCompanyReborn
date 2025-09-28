using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.Console.Interfaces
{
    using System;
    using TradingCompany.Console.Commands.Interfaces;

    public class InsertCommand<T> : ICommand where T : class
    {
        private readonly IGenericDAL<T> _dal;

        public InsertCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Insert a new {typeof(T).Name}";

        public void Execute()
        {
            var instance = Activator.CreateInstance<T>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanWrite)
                {
                    // Omit properties named ProfileId, UserId, RoleId, or of type DateTime
                    if (prop.Name == "Id" ||
                        prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        continue;
                    }
                    Console.Write($"Enter {prop.Name}: ");
                    var input = Console.ReadLine();

                    object convertedValue = null;
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    try
                    {
                        if (targetType == typeof(decimal) || targetType == typeof(double) || targetType == typeof(float))
                        {
                            convertedValue = decimal.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            convertedValue = Convert.ChangeType(input, targetType);
                        }
                        prop.SetValue(instance, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Invalid value for {prop.Name}. Error: {ex.Message}");
                        return;
                    }
                }
            }
            _dal.Create(instance);
            Console.WriteLine($"{typeof(T).Name} inserted successfully!");
        }
    }
}
