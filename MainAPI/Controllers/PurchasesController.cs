using MainAPI.Models;
using static MainAPI.Models.ErrorMessages;
using MainAPI.Utils.DBHandlers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace MainAPI.Controllers
{
    [Route("api/Purchases")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchasesHandler Handler;
        private readonly ProductsHandler PHandler;
        private readonly ProductCache ProductCache;
        public PurchasesController(PurchasesHandler _handler, ProductsHandler _phandler, ProductCache productCache)
        {
            Handler = _handler;
            PHandler = _phandler;
            ProductCache = productCache;
        }

        [Route("/Products/GetProducts")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            if (ProductCache.Cache is null || ProductCache.Cache.Count == 0) ProductCache.UpdateCache(PHandler);
            string JSONProductList = JsonConvert.SerializeObject(ProductCache.Cache);
            if (JSONProductList is null) return StatusCode(500, "Error retrieving product list from database");
            return Ok(JSONProductList);
        }

        [Authorize(Policy = "AdminRequired")]
        [Route("/Purchases/UserPurchases")]
        [HttpGet] //Must create a new endpoint for users to access their own purchases
        public IActionResult UserPurchases()
        {
            string? nameClaim = User.Identity.Name;
            if (nameClaim == null || nameClaim == "") return BadRequest(MEmptyClaim("Name"));
            List<Purchase>UserPurchases = Handler.GetUserPurchases(nameClaim);
            if (UserPurchases is null) { return NotFound(MNotFound(UserPurchases)); }
            return new JsonResult(UserPurchases);
        }

        [Authorize(Policy = "AdminRequired")]
        [Route("/Purchases/ShowPurchases")]
        [HttpGet] 
        public IActionResult ShowPurchases()
        {
            List<Purchase> Purchases = Handler.GetAllPurchases();
            if (Purchases is null) return NotFound(MNotFound(Purchases));
            return new JsonResult(Purchases);
        }

        [Authorize(Policy = "LoginRequired")]
        [Route("/Purchases/LogPurchase")]
        [HttpPost] //must change endpoint, handler and db so it uses cookie username
        public IActionResult LogPurchase(string JSONCart)
        {
            string? nameClaim = User.Identity.Name;
            if (nameClaim == null || nameClaim == "") return BadRequest(MEmptyClaim("Name"));
            List<Product>? Cart = JsonConvert.DeserializeObject<List<Product>>(JSONCart);
            if (ProductCache.Cache is null || ProductCache.Cache.Count == 0) ProductCache.UpdateCache(PHandler);
            Models.HttpReturn response = ProductCache.CheckCart(Cart);
            if (response.HttpCode != 200) return StatusCode(response.HttpCode, response.Message);
            if (!Handler.LogPurchase(Cart, nameClaim)) return StatusCode(500, MDBError);
            return Ok("Success");
        }

        [Authorize(Policy = "AdminRequired")]
        [Route("/Purchases/SetDelivery")]
        [HttpPut] //maybe rethink this and next endpoint? (low priority)
        public IActionResult SetDelivery(string JSONDelivery)
        {
            Delivery? delivery = JsonConvert.DeserializeObject<Delivery>(JSONDelivery);
            if (delivery is null) return BadRequest(MNullObject(delivery));
            if (Handler.SetDelivery(delivery)) return Ok(MOk);
            else return StatusCode(500, MDBError);
        }

        [Authorize(Policy = "AdminRequired")]
        [Route("/Purchases/SetPayment")]
        [HttpPut]
        public IActionResult SetPayment(string JSONPurchase)
        {
            Purchase? purchase = JsonConvert.DeserializeObject<Purchase>(JSONPurchase);
            if (purchase is null) return BadRequest(MNullObject(purchase));
            if (Handler.SetPayment(purchase)) return Ok(MOk);
            else return StatusCode(500, MDBError);
        }
    }
}
