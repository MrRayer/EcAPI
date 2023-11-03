namespace MainAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public Product(Nullable<int> _Id = null, string? _Name = null, string? _Description = null, decimal? _Price = null)
        {
            if (_Id is not null) { Id = _Id.Value; }
            if (_Name is not null) Name = _Name;
            if (_Description is not null) Description = _Description;
            if (_Price is not null) Price = _Price;
        }
        private Product() { }
    }
}
