using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using User.API.Data;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserContext _userContext;
        private ILogger<UsersController> _logger;

        public UsersController(UserContext userContext, ILogger<UsersController> logger)
        {
            _userContext = userContext;
            _logger = logger;
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
