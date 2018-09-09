using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using User.API.Controllers;
using User.API.Data;
using User.API.Models;
using Xunit;

namespace User.Api.XUnitTest
{
    public class UserApiUnitTest
    {
        private UserContext _userContext
        {
            get
            {
                var option = new DbContextOptionsBuilder<UserContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                var dbcontext = new UserContext(option);
                dbcontext.AppUsers.Add(new AppUser
                {
                    Name = "guoyunxi"
                });
                dbcontext.SaveChanges();
                return dbcontext;
            }
        }

        [Fact]
        public async Task 获取用户信息()
        {
            var loggerMoq = new Mock<ILogger<UsersController>>();
            var logger = loggerMoq.Object;
            var controller = new UsersController(_userContext, logger);
            IActionResult response = await controller.Get();
            response.ShouldBeOfType<OkObjectResult>();
        }
    }
}
