using MainAPI.Utils.DBHandlers;
using static MainAPI.Models.ErrorMessages;

namespace MainAPI.Models
{
    public class ProductCache
    {
        public List<Product> Cache { get; set; }
        public ProductCache() { Cache = new List<Product>(); }
        public void UpdateCache(ProductsHandler handler)
        {
            Cache = handler.GetProducts();
        }
        public HttpReturn CheckCart(List<Product> Cart)
        {
            foreach (Product product in Cart)
            {
                Product? cacheProduct = Cache.Find(item => item.Id == product.Id);
                if (cacheProduct is null) { return new HttpReturn(404, INIDB(product.Id)); }
                if (product.Price != cacheProduct.Price) return new HttpReturn(401, "cart prices does not match db prices");
            }
            return new HttpReturn(200,OkM);
        }
    }
}
