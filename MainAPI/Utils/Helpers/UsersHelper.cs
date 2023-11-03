namespace MainAPI.Utils.Helpers
{
    public class UsersHelper : IHelper
    {
        public string? ConnectionString { get; set; }
        public UsersHelper(IConfiguration config)
        {
            ConnectionString = config.GetValue<String>("ConnectionStrings:UsersDB");
        }
    }
}
