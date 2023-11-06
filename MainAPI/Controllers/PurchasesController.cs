using MainAPI.Models;
using static MainAPI.Models.ErrorMessages;
using MainAPI.Utils.DBHandlers;
using MainAPI.Utils.Helpers;
using Microsoft.AspNetCore.Http;
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
        public PurchasesController(PurchasesHandler _handler)
        {
            Handler = _handler;
        }
        [Authorize(Policy = "AdminRequired")]
        [Route("/Purchases/UserPurchases")]
        [HttpGet] //Must create a new endpoint for users to access their own purchases
        public IActionResult UserPurchases(string JSONId)
        {
            User? Id = JsonConvert.DeserializeObject<User>(JSONId);
            if (Id is null) return BadRequest(MNullObject(Id));
            List<Purchase>UserPurchases = Handler.GetUserPurchases(Id);
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
        public IActionResult LogPurchase(string JSONPurchase, string JSONDelivery)
        {
            Purchase? purchase = JsonConvert.DeserializeObject<Purchase>(JSONPurchase);
            List<Delivery>? delivery = JsonConvert.DeserializeObject<List<Delivery>>(JSONDelivery);
            if (purchase is null) return BadRequest(MNullObject(purchase));
            if (delivery is null) return BadRequest(MNullObject(delivery));
            if (Handler.LogPurchase(purchase, delivery)) return Ok(MOk);
            else return StatusCode(500, MDBError);
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
