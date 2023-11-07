using MainAPI.Models;
using MainAPI.Utils.DBHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using static MainAPI.Models.ErrorMessages;

namespace MainAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersHandler Handler;
        public UsersController(UsersHandler _handler)
        {
            Handler = _handler;
        }

        [Route("/Users/Login")]
        [HttpGet]
        public async Task<IActionResult> Login(string JSONUser)
        {
            User? user = JsonConvert.DeserializeObject<User>(JSONUser);
            if (user is null) return BadRequest(MNullObject(user));
            user = Handler.ValidateUser(user);
            if (user.Role == "invalid") return StatusCode(401, MNotValidated);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var identity = new ClaimsIdentity(claims, "UserAuthentication");
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync("UserAuthentication", principal);
            return Ok(MOk);
        }

        [Authorize(Policy = "LoginRequired")]
        [Route("/Users/GetUser")]
        [HttpGet]
        public IActionResult GetUser()
        {
            string? nameClaim = User.Identity.Name;
            if (nameClaim == null || nameClaim == "") return BadRequest(MEmptyClaim("Name"));
            User userInfo = Handler.GetUser(nameClaim);
            if (userInfo.CheckNewUser().HttpCode == 400) return NotFound(MNotFound(userInfo));
            return new JsonResult(userInfo);
        }

        [Route("/Users/CreateUser")]
        [HttpPost]
        public IActionResult CreateUser(string JSONUser)
        {
            User? user = JsonConvert.DeserializeObject<User>(JSONUser);
            if (user is null) return BadRequest(MNullObject(user));
            HttpReturn userCheck = user.CheckNewUser();
            if (userCheck.HttpCode != 200) return StatusCode(userCheck.HttpCode,userCheck.Message);
            if (Handler.AddUser(user)) return Ok(MOk);
            else return StatusCode(500,MDBError);            
        }

        [Authorize(Policy = "LoginRequired")]
        [Route("/Users/UpdateUser")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(string JSONUpdates)
        {
            string? nameClaim = User.Identity.Name;
            if (nameClaim == null || nameClaim == "") return BadRequest(MEmptyClaim("Name"));
            User? updates = JsonConvert.DeserializeObject<User>(JSONUpdates);
            if (updates is null) return BadRequest(MNullObject(updates));
            if (!Handler.UpdateUser(nameClaim, updates)) return NotFound(MNotFound(nameClaim));
            ClaimsIdentity currentIdentity = (ClaimsIdentity)User.Identity;
            ClaimsIdentity newIdentity = new(
                currentIdentity.Claims,
                currentIdentity.AuthenticationType,
                ClaimTypes.Name, updates.Username
            );
            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync(new ClaimsPrincipal(newIdentity));
            return Ok(MOk);
        }

        [Authorize(Policy = "MustBeAdmin")]
        [Route("/Users/DeleteUser")]
        [HttpDelete]
        public IActionResult DeleteUser(string JSONUser)
        {
            User? user = JsonConvert.DeserializeObject<User>(JSONUser);
            if (user is null) return BadRequest(MNullObject(user));
            if (Handler.DeleteUser(user)) return Ok(MOk);
            else return NotFound(MNotFound(user));
        }
    }
}
