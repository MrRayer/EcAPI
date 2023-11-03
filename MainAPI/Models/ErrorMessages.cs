namespace MainAPI.Models
{
    public class ErrorMessages
    {
        public static string OkM => "Success";
        public static string DBM => "DB SP error";
        public static string UVM => "User validated";
        public static string UNVM => "User NOT validated";
        public static string IIM => "Insuficient information";
        static public string NONM(Object obj) { return $"{obj.GetType().Name} null or does not match model"; }
        static public string NFM(Object obj) { return $"{obj.GetType().Name} not found"; }
        static public string INIDB(int id) { return $"item id: {id}, not in the db"; }
    }
}
