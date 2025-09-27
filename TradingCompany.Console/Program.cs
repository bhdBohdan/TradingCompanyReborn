

namespace TradingCompany.Console
{
    using System;
    using TradingCompany.DAL.Concrete;
    using TradingCompany.DALEF.Concrete;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    internal class Program
    {
        // static readonly string CONNECTION_STRING = "Data Source=localhost,1433;Database=TradingCompany;Persist Security Info=False;User ID=sa;Password=MyStr0ng!Pass123;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=30";
        static readonly string CONNECTION_STRING = "Data Source = localhost,1433; Database=TradingCompany;User ID = sa; Password=MyStr0ng!Pass123;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to TradingCompany!");
            char c = 's';

            while (c != 'q' && c != 'Q')
            {
                switch (c)
                {
                    case '1':
                        Console.WriteLine("You chose to get all Users.");

                        GetAllUsers();
                        break;
                    case '2':
                        Console.WriteLine("You chose to get all Roles.");
                        break;
                    case '3':
                        Console.WriteLine("You chose to insert a user.");

                        InsertUser();
                        break;
                    case 'q':
                        Console.WriteLine("You chose to Quit.");
                        break;
                    default:
                        if (c != 's')
                        {
                            Console.WriteLine("Invalid choice. Please try again.");
                        }
                        break;
                }

                Console.WriteLine("\nType:\n1 to get all Users;\n2 to get all Roles;\n3 to insert a user;\nq to Quit.");

                c = Console.ReadLine()[0];
            }
        }

        private static void InsertUser()
        {
            //var dal = new UserDAL();

            //var oldUser = new User
            //{

            //};

            //var newUser = dal.Register(oldUser);

            //Console.WriteLine($"Inserted User: {newUser.UserId}: {newUser.Title} - {newUser.Role.Name} - {newUser.ReleaseDate.ToShortDateString()}");
        }

        private static void GetAllUsers()
        {
            var dal = new UserDALEF(CONNECTION_STRING);
            var users = dal.GetAll();
            Console.WriteLine(users.Count);

            foreach (var user in users)
            {
                Console.WriteLine($"{user.UserId}: {user.Username} - {user.PasswordHash} - {user.RegistrationDate.ToShortDateString()}");
            }
        }

        private static void DeleteMovie()
        {

            Console.WriteLine("Enter the MovieId to delete");

            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                var dal = new UserDAL(CONNECTION_STRING);

                bool success = false;
                try
                {
                    success = dal.Delete(userId);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Fail to delete, {e.Message}");
                }
                if (success)
                {
                    Console.WriteLine($"User with {userId} deleted succesfully");
                }
                else
                {
                    Console.WriteLine("Fail to delete, user not found");
                }
            }
            else
            {
                Console.WriteLine("Invalid Id");

            }

        }
    }
}
