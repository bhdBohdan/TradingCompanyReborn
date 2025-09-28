using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DTO;
using AutoMapper;


namespace TradingCompany.DAL.Concrete
{
    public class UserDAL : GenericDAL<User>, IUserDAL
    {

        public UserDAL(string connStr, IMapper mapper) : base(connStr, mapper) { }



        public override bool Delete(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "DELETE FROM Users WHERE UserID = @delID ";

                command.Parameters.AddWithValue("@delID", userId);
                //command.Parameters.AddWithValue("@genreId", user.Genre.GenreId);
                //command.Parameters.AddWithValue("@date", user.ReleaseDate);

                int affectedRows = command.ExecuteNonQuery();

                //user.UserId = (int)command.ExecuteScalar();

                //connection.Close();

            }
            return true;

        }
        public override List<User> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();


                command.CommandText = "SELECT * FROM Users";

                SqlDataReader reader = command.ExecuteReader();

                List<User> users = new List<User>();

                while (reader.Read())
                {
                    Console.WriteLine(reader["UserID"]);
                    User user = new User
                    {
                        Id = (int)reader["UserID"],
                        Email = (string)reader["Email"],
                        PasswordHash = (string)reader["PasswordHash"],
                        RestoreKeyword = (string)reader["RestoreKeyword"],
                        Username = (string)reader["Username"],
                        RegistrationDate = (DateTime)reader["CreatedAt"]
                    };
                    users.Add(user);
                }
                connection.Close();
                return users;
            }

        }

        public override User GetById(int userId)
        {
            throw new NotImplementedException();
        }

        public override User Create(User user)
        {
            //SqlConnection connection = new SqlConnection("Data Source=localhost,1433;Persist Security Info=False;User ID=sa;Password=MyStr0ng!Pass123;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=30");

            //using (SqlConnection connection = new SqlConnection(_connStr))
            //using (SqlCommand command = connection.CreateCommand())
            //{
            //connection.Open();

            //SqlCommand command = connection.CreateCommand();
            //command.CommandText = "INSERT INTO Users (Title, GenreId, ReleaseDate) OUTPUT inserted.UserId VALUES (@title, @genreId, @date)";

            //command.Parameters.AddWithValue("@title", user.Title);
            //command.Parameters.AddWithValue("@genreId", user.Genre.GenreId);
            //command.Parameters.AddWithValue("@date", user.ReleaseDate);

            ////int affectedRows = command.ExecuteNonQuery();

            //user.UserId = (int)command.ExecuteScalar();

            //connection.Close();
            throw new NotImplementedException();
            //return user;
        }

        public override User Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
