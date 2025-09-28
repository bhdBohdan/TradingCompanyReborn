
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.Console.Interfaces { 
    using System;
    using TradingCompany.Console.Commands.Interfaces;

    public class GetAllCommand<T> : ICommand where T : class
    {
        private readonly IGenericDAL<T> _dal;

        public GetAllCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Get all {typeof(T).Name}s";

        public void Execute()
        {
            var items = _dal.GetAll();
            Console.WriteLine($"Total {typeof(T).Name}s: {items.Count}");
            foreach (var item in items)
            {
                Console.WriteLine(item); // override ToString() in entity class for proper display
            }
        }
    }

}
