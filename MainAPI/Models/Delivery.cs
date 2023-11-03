namespace MainAPI.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; }
        public Delivery(Nullable<int> _Id = null, Nullable<int> _PurchaseId = null,
            Nullable<int> _UserId = null, Nullable<int> _ProductId = null, Nullable<bool> _Status = null)
        {
            if (_Id is not null) Id = _Id.Value;
            if (_PurchaseId is not null) PurchaseId = _PurchaseId.Value;
            if (_UserId is not null) UserId = _UserId.Value;
            if (_ProductId is not null) ProductId = _ProductId.Value;
            if (_Status is not null) { Status = _Status.Value; }
        }
        private Delivery() { }
    }
}
