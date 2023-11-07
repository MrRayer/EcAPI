using MainAPI.Models;
using MainAPI.Utils.DBHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static MainAPI.Models.ErrorMessages;

namespace MainAPI.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsHandler Handler;
        public ProductsController(ProductsHandler _handler)
        {
            Handler = _handler;
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
        [HttpDelete]
        public IActionResult DeleteProduct(string JSONId)
        {
            Product? Id = JsonConvert.DeserializeObject<Product>(JSONId);
            if (Id is null) return BadRequest(MNullObject(Id));
            if (Handler.DeleteProduct(Id)) return Ok(MOk);
            else return StatusCode(501, MDBError);
        }
    }
}
