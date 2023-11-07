using Dapper;
using MainAPI.Models;
using MainAPI.Utils.Helpers;
using System.Data.SqlClient;

namespace MainAPI.Utils.DBHandlers
{
    public class PurchasesHandler
    {
        private PurchasesHelper Helper { get; set; }
        public PurchasesHandler(PurchasesHelper _Helper)
        {
            Helper = _Helper;
        }

        internal List<Purchase> GetUserPurchases(string User)
        {
            List<Purchase>? response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                response = conn.Query<Purchase>($"dbo.GetUserPurchases '{User}'").ToList();
            }
            response ??= new List<Purchase>();
            return response;
        }

        internal List<Purchase> GetAllPurchases()
        {
            List<Purchase>? response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                response = conn.Query<Purchase>($"dbo.GetAllPurchases").ToList();
            }
            response ??= new List<Purchase>();
            return response;
        }

        internal bool SetDelivery(Delivery delivery)
        {
            bool response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.SetDelivery '{delivery.Id}','{delivery.Status}'").FirstOrDefault();
            }
            return response;
        }

        internal bool SetPayment(Purchase purchase)
        {
            bool response;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.SetPayment '{purchase.Id}','{purchase.PaymentStatus}'").FirstOrDefault();
            }
            return response;
        }

        internal bool LogPurchase(List<Product> Cart, string Username)
        {
            bool deliveryResponse = true;
            using (SqlConnection conn = new (Helper.ConnectionString))
            {
                int id = conn.Query<int>($"dbo.AddPurchase '{Username}'").FirstOrDefault();
                foreach (Product product in Cart)
                {
                    if (!conn.Query<bool>($"dbo.AddDelivery '{id}','{Username}','{product.Id}'").FirstOrDefault()) deliveryResponse = false;
                }
            }
            bool response;
            if (deliveryResponse) response = true; else response = false;
            return response;
        }
    }
}
