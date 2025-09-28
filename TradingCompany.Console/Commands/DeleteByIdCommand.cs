using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.Console.Interfaces
{
    using System;

    public class DeleteByIdCommand<T> where T : class
    {
        private readonly IGenericDAL<T> _dal;

        public DeleteByIdCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Delete {typeof(T).Name} by ID";

        public void Execute()
        {
            Console.Write("Enter ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var success = _dal.Delete(id);
                if (success)
                {
                    Console.WriteLine($"{typeof(T).Name} with ID {id} deleted.");
                }
                else
                {
                    Console.WriteLine($"{typeof(T).Name} with ID {id} not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }
    }
}
