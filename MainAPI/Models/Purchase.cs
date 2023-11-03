namespace MainAPI.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? PaypalToken { get; set; }
        public bool PaymentStatus { get; set; }
        public bool Status { get; set; }
        public Purchase(Nullable<int> _Id = null, Nullable<int> _UserId = null,
            string? _PaypalToken = null, Nullable<bool> _PaymentStatus = null, Nullable<bool> _Status = null)
        {
            if (_Id is not null) Id = _Id.Value;
            if (_UserId is not null) UserId = _UserId.Value;
            PaypalToken = _PaypalToken;
            if (_PaymentStatus is not null) PaymentStatus = _PaymentStatus.Value;
            if (_Status is not null) Status = _Status.Value;
        }
        private Purchase() { }
    }
}
