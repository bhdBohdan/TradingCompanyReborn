using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompany.Console.Interfaces;

namespace TradingCompany.Console.AppMenu
{
    using AutoMapper;
    using System;
    using TradingCompany.DALEF.Concrete;
    using TradingCompany.DTO;

    internal class AppMenu
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public AppMenu(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _mapper = mapper;
        }
        public void Show()
        {
            Console.WriteLine("Welcome to TradingCompany!\n");
            char choice = ' ';
            while (choice != 'q' && choice != 'Q')
            {
               

                Console.WriteLine("1 - UserMenu");
                Console.WriteLine("2 - CustomerMenu");
                Console.WriteLine("3 - RoleMenu");
                Console.WriteLine("4 - ProductMenu");
                Console.WriteLine("5 - OrderMenu");
                Console.WriteLine("q - Quit\n");

                choice = Console.ReadLine()[0];

                if (choice == '1')
                {
                    new Menu<User>(new UserDALEF(_connectionString, _mapper)).Show();
                }

                else if (choice == '2')
                { new Menu<UserProfile>(new UserProfileDALEF(_connectionString, _mapper)).Show(); }
                else if (choice == '3')
                {
                    new Menu<Role>(new RoleDALEF(_connectionString, _mapper)).Show();
                }

                else if (choice == '4')
                { new Menu<Product>(new ProductDALEF(_connectionString, _mapper)).Show(); }
                else if (choice == '5')
                { new Menu<Order>(new OrderDALEF(_connectionString, _mapper)).Show(); }

                else if (choice != 'q' && choice != 'Q')
                    Console.WriteLine("Invalid choice. Try again.");
            }
        }
    }
}
