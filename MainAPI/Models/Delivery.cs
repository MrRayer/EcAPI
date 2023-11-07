namespace MainAPI.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public string Username { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; }
        public Delivery(int purchaseId, string username, int productId)
        {
            PurchaseId = purchaseId;
            Username = username;
            ProductId = productId;
            Status = false;
        }
        private Delivery() { }
    }
}
