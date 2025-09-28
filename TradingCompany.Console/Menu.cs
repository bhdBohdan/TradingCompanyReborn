using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.Console.Interfaces;

namespace TradingCompany.Console
{
    using System;
    using TradingCompany.DAL.Interfaces;

    public class Menu<T> where T : class
    {
        private readonly GetAllCommand<T> _getAllCommand;
        private readonly InsertCommand<T> _insertCommand;
        private readonly GetByIdCommand<T> _getByIdCommand;
        private readonly DeleteByIdCommand<T> _deleteByIdCommand;
        private readonly UpdateCommand<T> _updateCommand;

        public Menu(IGenericDAL<T> dal)
        {
            _getAllCommand = new GetAllCommand<T>(dal);
            _insertCommand = new InsertCommand<T>(dal);
            _getByIdCommand = new GetByIdCommand<T>(dal);
            _deleteByIdCommand = new DeleteByIdCommand<T>(dal);
            _updateCommand = new UpdateCommand<T>(dal);
        }
     

        public void Show()
        {
            char choice = ' ';
            while (choice != 'q' && choice != 'Q')
            {
                Console.WriteLine($"\n{typeof(T).Name} Menu:");
               
                Console.WriteLine("1 - " + _getAllCommand.Description);
                Console.WriteLine("2 - " + _getByIdCommand.Description);
                Console.WriteLine("3 - " + _insertCommand.Description);
                Console.WriteLine("4 - " + _deleteByIdCommand.Description);
                Console.WriteLine("5 - " + _updateCommand.Description);
                Console.WriteLine("q - Quit");

                choice = Console.ReadLine()[0];

                if (choice == '1')
                    _getAllCommand.Execute();
                else if (choice == '2')
                    _getByIdCommand.Execute();
                else if (choice == '3')
                    _insertCommand.Execute();
                else if (choice == '4')
                    _deleteByIdCommand.Execute();
                else if (choice == '5')
                    _updateCommand.Execute();
                else if (choice != 'q' && choice != 'Q')
                    Console.WriteLine("Invalid choice. Try again.");
            }
        }
    }

}
