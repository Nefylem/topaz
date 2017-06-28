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

        public ActionResult CheckLogin(string user, string password)
        {
            var ip = Request.ServerVariables["REMOTE_ADDR"];
            return Json(Topaz.Login.CheckLogin(user, password, ip));
        }
    }
}