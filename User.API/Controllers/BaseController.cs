using Microsoft.AspNetCore.Mvc;
using User.API.Dtos;

namespace User.API.Controllers
{
    public class BaseController : Controller
    {
        public UserDto UserDto => new UserDto{ Id = 1,Name = "guoyunxi"};
    }
}