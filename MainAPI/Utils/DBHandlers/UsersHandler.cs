using MainAPI.Models;
using MainAPI.Utils.Helpers;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace MainAPI.Utils.DBHandlers
{
    public class UsersHandler
    {
        private UsersHelper Helper { get; set; }
        public UsersHandler(UsersHelper _Helper)
        {
            Helper = _Helper;
        }

        internal bool AddUser(User user)
        {
            bool response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.addUser '{user.Username}','{user.Password}','{user.Email}','{user.Address}'").FirstOrDefault();
            }
            return response;
        }

        internal User ValidateUser(User user)
        {
            string? response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                response = conn.Query<string>($"dbo.validateUser '{user.Username}','{user.Password}'").FirstOrDefault();
            }
            if (response is null || response is "") response = "invalid"; 
            user.Role = response;
            return user;
        }

        internal bool UpdateUser(string user, User updates)
        {
            bool response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.updateUser '{user}'," +
                    $"'{updates.Username}','{updates.Password}'," +
                    $"'{updates.Email}',''{updates.Address}").FirstOrDefault();
            }
            return response;
        }

        internal User GetUser(string user)
        {
            User? response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                var parameters = new { Username = user };
                response = conn.Query<User>($"dbo.GetUser",parameters,commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            response ??= new User();
            return response;
        }

        internal bool DeleteUser(User user)
        {
            bool response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.DeleteUser '{user.Username}','{user.Password}'").FirstOrDefault();
            }
            return response;
        }
    }
}