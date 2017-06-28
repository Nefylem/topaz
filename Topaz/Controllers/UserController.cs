using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topaz.Application;
using Topaz.Application.Login.Dto;

namespace Topaz.Controllers
{
    public class UserController : TopazController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNewUser(UserDto.NewUser newUser)
        {
            var result = Topaz.Login.CreateNewUser(newUser);
            return Json(result.Result);
        }
    }
}