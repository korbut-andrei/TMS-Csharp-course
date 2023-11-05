using Lesson_10_ASP.NET_Core.Models;
using Lesson_10_ASP.NET_Core.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lesson_10_ASP.NET_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        [HttpGet]
        public UserModel[] Get() 
        {
            return _userService.GetAllUsers();
        }
        
        
        // POST api/<ValuesController1>
        [HttpPost]
        public IActionResult Post([FromBody] NewUserModel model)
        {
            var result = _userService.AddUser(model);
            
            if (!result)
            {
                return BadRequest(new {error = true });
            }

            return Ok(new { error = false });
        }

        // PUT api/<ValuesController1>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ValuesController1>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
