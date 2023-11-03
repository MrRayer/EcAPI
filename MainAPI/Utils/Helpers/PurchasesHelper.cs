namespace MainAPI.Utils.Helpers
{
    public class PurchasesHelper : IHelper
    {
        public string? ConnectionString { get; set; }
        public PurchasesHelper(IConfiguration config)
        {
            ConnectionString = config.GetValue<String>("ConnectionStrings:PurchasesDB");
        }
    }
}
