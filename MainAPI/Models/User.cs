using System.Text.Json.Serialization;

namespace MainAPI.Models
{
    public class User
    {
        public Nullable<int> Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public User(){}
        public HttpReturn CheckNewUser()
        {
            if (NullOrEmpty(Username)) return new HttpReturn(400, "Username null or empty");
            if (NullOrEmpty(Password)) return new HttpReturn(400, "Password null or empty");
            if (NullOrEmpty(Email)) return new HttpReturn(400, "Email null or empty");
            if (NullOrEmpty(Address)) return new HttpReturn(400, "Address null or empty");
            else return new HttpReturn(200,"Success");
        }
        private static bool NullOrEmpty(string attribute)
        {
            if (attribute is "" || attribute is null) return true;
            return false;
        }
    }
}
