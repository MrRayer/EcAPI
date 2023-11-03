using Dapper;
using MainAPI.Models;
using MainAPI.Utils.Helpers;
using System.Data.SqlClient;

namespace MainAPI.Utils.DBHandlers
{
    public class ProductsHandler
    {
        private ProductsHelper Helper { get; set; }
        public ProductsHandler(ProductsHelper _Helper) 
        {
            Helper = _Helper;
        }
        internal bool AddProduct(Product product)
        {
            bool response;
            using (SqlConnection conn = new(Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.addProduct '{product.Name}'," +
                    $"'{product.Description}','{product.Price}'").FirstOrDefault();
            }
            return response;
        }
        internal bool UpdateProduct(Product product)
        {
            bool response;
            using (SqlConnection conn = new(Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.UpdateProduct '{product.Id}','{product.Name}'," +
                    $"'{product.Description}','{product.Price}'").FirstOrDefault();
            }
            return response;
        }
        internal bool DeleteProduct(Product product)
        {
            bool response;
            using (SqlConnection conn = new(Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.DeleteProduct '{product.Id}'").FirstOrDefault();
            }
            return response;
        }
        internal List<Product> GetProducts()
        {
            List<Product>? response;
            using (SqlConnection conn = new(Helper.ConnectionString))
            {
                response = conn.Query<Product>($"dbo.GetProducts").ToList();
            }
            response ??= new List<Product>();
            return response;
        }
    }
}