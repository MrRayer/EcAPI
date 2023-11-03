using MainAPI.Models;
using static MainAPI.Models.ErrorMessages;
using MainAPI.Utils.DBHandlers;
using MainAPI.Utils.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [Route("/Purchases/UserPurchases")]
        [HttpGet]
        public IActionResult UserPurchases(string JSONId)
        {
            User? Id = JsonConvert.DeserializeObject<User>(JSONId);
            if (Id is null) return BadRequest(NONM(Id));
            List<Purchase>UserPurchases = Handler.GetUserPurchases(Id);
            if (UserPurchases is null) { return NotFound(NFM(UserPurchases)); }
            return new JsonResult(UserPurchases);
        }

        [Route("/Purchases/ShowPurchases")]
        [HttpGet]
        public IActionResult ShowPurchases()
        {
            List<Purchase> Purchases = Handler.GetAllPurchases();
            if (Purchases is null) return NotFound(NFM(Purchases));
            return new JsonResult(Purchases);
        }

        [Route("/Purchases/LogPurchase")]
        [HttpPost]
        public IActionResult LogPurchase(string JSONPurchase, string JSONDelivery)
        {
            Purchase? purchase = JsonConvert.DeserializeObject<Purchase>(JSONPurchase);
            List<Delivery>? delivery = JsonConvert.DeserializeObject<List<Delivery>>(JSONDelivery);
            if (purchase is null) return BadRequest(NONM(purchase));
            if (delivery is null) return BadRequest(NONM(delivery));
            if (Handler.LogPurchase(purchase, delivery)) return Ok(OkM);
            else return StatusCode(500, DBM);
        }

        [Route("/Purchases/SetDelivery")]
        [HttpPut]
        public IActionResult SetDelivery(string JSONDelivery)
        {
            Delivery? delivery = JsonConvert.DeserializeObject<Delivery>(JSONDelivery);
            if (delivery is null) return BadRequest(NONM(delivery));
            if (Handler.SetDelivery(delivery)) return Ok(OkM);
            else return StatusCode(500, DBM);
        }

        [Route("/Purchases/SetPayment")]
        [HttpPut]
        public IActionResult SetPayment(string JSONPurchase)
        {
            Purchase? purchase = JsonConvert.DeserializeObject<Purchase>(JSONPurchase);
            if (purchase is null) return BadRequest(NONM(purchase));
            if (Handler.SetPayment(purchase)) return Ok(OkM);
            else return StatusCode(500, DBM);
        }
    }
}
