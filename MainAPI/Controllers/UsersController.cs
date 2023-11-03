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
            if (user is null) return BadRequest(NONM(user));
            user = Handler.ValidateUser(user);
            if (user.Role == "invalid") return StatusCode(401, UNVM);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var identity = new ClaimsIdentity(claims, "UserAuthentication");
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync("UserAuthentication", principal);
            return Ok(OkM);
        }

        [Route("/Users/GetUser")]
        [HttpGet]
        public IActionResult GetUser(string JSONUser)
        {
            User? user = JsonConvert.DeserializeObject<User>(JSONUser);
            if (user is null) return BadRequest(NONM(user));
            User userInfo = Handler.GetUser(user);
            if (userInfo.CheckNewUser().HttpCode == 400) return NotFound(NFM(userInfo));
            return new JsonResult(userInfo);            
        }

        [Route("/Users/CreateUser")]
        [HttpPost]
        public IActionResult CreateUser(string JSONUser)
        {
            User? user = JsonConvert.DeserializeObject<User>(JSONUser);
            if (user is null) return BadRequest(NONM(user));
            HttpReturn userCheck = user.CheckNewUser();
            if (userCheck.HttpCode != 200) return StatusCode(userCheck.HttpCode,userCheck.Message);
            if (Handler.AddUser(user)) return Ok(OkM);
            else return StatusCode(500,DBM);
            
        }

        [Route("/Users/UpdateUser")]
        [HttpPut]
        public IActionResult UpdateUser(string JSONUser, string JSONUpdates)
        {
            User? user = JsonConvert.DeserializeObject<User>(JSONUser);
            User? updates = JsonConvert.DeserializeObject<User>(JSONUpdates);
            if (user is null) return BadRequest(NONM(user));
            if (updates is null) return BadRequest(NONM(updates));
            if (Handler.UpdateUser(user,updates)) return Ok(OkM);
            else return NotFound(NFM(user));
        }

        [Route("/Users/DeleteUser")]
        [HttpDelete]
        public IActionResult DeleteUser(string JSONUser)
        {
            User? user = JsonConvert.DeserializeObject<User>(JSONUser);
            if (user is null) return BadRequest(NONM(user));
            if (Handler.DeleteUser(user)) return Ok(OkM);
            else return NotFound(NFM(user));
        }

        [Authorize(Policy = "MustBeLoged")]
        [Route("/Users/testEndpoint")]
        [HttpGet]
        public IActionResult TestEndpoint()
        {
            User test1 = new(){Id = 1, Username = "pepe", Password = "asd123",
                Email = "pepemail@mail.com", Address = "fakestreet 123"};
            return new JsonResult(test1);
        }
        [Route("/Users/AccessDenied")]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return StatusCode(403, "Forbidden");
        }
    }
}
