using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace E_Commerce.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    [Authorize]
    public class DemoController : ControllerBase
    {

        [HttpGet("HelloWorld")]
        [Authorize(Roles = "User, Admin, Seller")]
        public string PrintHelloWorld()
        {
            return "HelloWorld";
        }

        [HttpGet("HelloAdmin")]
        [Authorize(Roles = "Admin")]
        public string PrintHelloAdmin()
        {
            return "Hello Admin!";
        }

        [HttpGet("HelloModerator")]
        [Authorize(Roles = "Seller, Admin")]
        public string PrintHelloModerator()
        {
            return "Hello Moderator!";
        }
    }
}
