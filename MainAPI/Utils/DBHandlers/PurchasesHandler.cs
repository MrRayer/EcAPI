using Dapper;
using MainAPI.Models;
using MainAPI.Utils.Helpers;
using System.Data.SqlClient;
using System.Diagnostics;

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
            using (SqlConnection conn = new SqlConnection(Helper.ConnectionString))
            {
                response = conn.Query<Purchase>($"dbo.GetUserPurchases '{User}'").ToList();
            }
            if (response is null) response = new List<Purchase>();
            return response;
        }

        internal List<Purchase> GetAllPurchases()
        {
            List<Purchase>? response;
            using (SqlConnection conn = new SqlConnection(Helper.ConnectionString))
            {
                response = conn.Query<Purchase>($"dbo.GetAllPurchases").ToList();
            }
            if (response is null) response = new List<Purchase>();
            return response;
        }

        internal bool SetDelivery(Delivery delivery)
        {
            bool response;
            using (SqlConnection conn = new SqlConnection(Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.SetDelivery '{delivery.Id}','{delivery.Status}'").FirstOrDefault();
            }
            return response;
        }

        internal bool SetPayment(Purchase purchase)
        {
            bool response;
            using (SqlConnection conn = new SqlConnection(Helper.ConnectionString))
            {
                response = conn.Query<bool>($"dbo.SetPayment '{purchase.Id}','{purchase.PaymentStatus}'").FirstOrDefault();
            }
            return response;
        }

        internal bool LogPurchase(Purchase purchase, List<Delivery> deliveryList)
        {
            bool deliveryResponse = true;
            using (SqlConnection conn = new SqlConnection(Helper.ConnectionString))
            {
                int id = conn.Query<int>($"dbo.AddPurchase '{purchase.UserId}','{purchase.PaypalToken}'").FirstOrDefault();
                foreach (Delivery delivery in deliveryList)
                {
                    if (!conn.Query<bool>($"dbo.AddDelivery '{id}','{delivery.UserId}','{delivery.ProductId}'").FirstOrDefault()) deliveryResponse = false;
                }
            }
            bool response;
            if (deliveryResponse) response = true; else response = false;
            return response;
        }
    }
}
