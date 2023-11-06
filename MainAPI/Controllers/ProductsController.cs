using MainAPI.Models;
using MainAPI.Utils.DBHandlers;
using MainAPI.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using static MainAPI.Models.ErrorMessages;

namespace MainAPI.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsHandler Handler;
        private readonly ProductCache ProductCache;
        public ProductsController(ProductsHandler _handler, ProductCache _ProductCache)
        {
            Handler = _handler;
            ProductCache = _ProductCache;
        }

        [Authorize(Policy = "LoginRequired")]
        [Route("/Products/CheckCart")]
        [HttpGet]
        public IActionResult CheckCart(string JSONCart)
        {
            List<Product>? Cart = JsonConvert.DeserializeObject<List<Product>>(JSONCart);
            if (Cart != null) return BadRequest(MNullObject(Cart));
            if (ProductCache.Cache is null || ProductCache.Cache.Count == 0) ProductCache.UpdateCache(Handler);
            Models.HttpReturn response = ProductCache.CheckCart(Cart);
            return StatusCode(response.HttpCode, response.Message);
        }

        [Route("/Products/GetProducts")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            if (ProductCache.Cache is null || ProductCache.Cache.Count == 0) ProductCache.UpdateCache(Handler);
            string JSONProductList = JsonConvert.SerializeObject(ProductCache.Cache);
            if (JSONProductList is null) return StatusCode(500, "Error retrieving product list from database");
            return Ok(JSONProductList);
        }

        [Authorize(Policy = "AdminRequired")]
        [Route("/Products/AddProduct")]
        [HttpPost]
        public IActionResult AddProduct(string JSONProduct)
        {
            Product? product = JsonConvert.DeserializeObject<Product>(JSONProduct);
            if (product is null) return BadRequest(MNullObject(product));
            if (Handler.AddProduct(product)) return Ok(MOk);
            else return StatusCode(501,MDBError);
        }

        [Authorize(Policy = "AdminRequired")]
        [Route("/Products/UpdateProduct")]
        [HttpPut]
        public IActionResult UpdateProduct(string JSONProduct)
        {
            Product? Updates = JsonConvert.DeserializeObject<Product>(JSONProduct);
            if (Updates is null) return BadRequest(MNullObject(Updates));
            if (Handler.UpdateProduct(Updates)) return Ok(MOk);
            else return StatusCode(501, MDBError);
        }

        [Authorize(Policy = "AdminRequired")]
        [Route("/Products/DeleteProduct")]
        [HttpDelete] // must change endpoint, handler and dbsp to use cookie name instead of id
        public IActionResult DeleteProduct(string JSONId)
        {
            Product? Id = JsonConvert.DeserializeObject<Product>(JSONId);
            if (Id is null) return BadRequest(MNullObject(Id));
            if (Handler.DeleteProduct(Id)) return Ok(MOk);
            else return StatusCode(501, MDBError);
        }
    }
}
