namespace MainAPI.Models
{
    public class ErrorMessages
    {
        public static string MOk => "Success";
        public static string MDBError => "DB SP error";
        public static string MValidated => "User validated";
        public static string MNotValidated => "User NOT validated";
        public static string MLackInfo => "Insuficient information";
        static public string MNullObject(Object obj) { return $"{obj.GetType().Name} null or does not match model"; }
        static public string MNotFound(Object obj) { return $"{obj.GetType().Name} not found"; }
        static public string MIdNotFound(int id) { return $"item id: {id}, not in the db"; }
        static public string MEmptyClaim(string claim) { return $"could not find {claim}, try reloggin"; }
    }
}
