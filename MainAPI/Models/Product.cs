namespace MainAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public Product(string name, decimal price, string description = "")
        {
            Name = name;
            Price = price;
            Description = description;
        }
        private Product() { }
    }
}
