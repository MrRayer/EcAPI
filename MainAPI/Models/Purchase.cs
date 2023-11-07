namespace MainAPI.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? PaypalToken { get; set; }
        public bool PaymentStatus { get; set; }
        public bool Status { get; set; }
        public Purchase(string username, string paypalToken = "")
        {
            if (username == "") throw new Exception("Empty username");
            Username = username;
            PaypalToken = paypalToken;
            PaymentStatus = false;
            Status = false;
        }
        private Purchase() { }
    }
}
