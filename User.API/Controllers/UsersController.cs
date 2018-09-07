using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using User.API.Data;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private UserContext _userContext;

        public UsersController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userContext.AppUsers.ToArrayAsync();
            return Ok(JsonConvert.SerializeObject(users));
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userContext.AppUsers.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                throw new UserOperationException($"错误的用户Id");
            }
            return Json(user);
        }
    }
}
