namespace MainAPI.Utils.Helpers
{
    public class ProductsHelper : IHelper
    {
        public string? ConnectionString { get; set; }
        public ProductsHelper(IConfiguration config)
        {
            ConnectionString = config.GetValue<String>("ConnectionStrings:ProductsDB");
        }
    }
}
