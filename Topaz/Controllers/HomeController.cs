using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topaz.Application;

namespace Topaz.Controllers
{
    public class HomeController : TopazController
    {
        public ActionResult Index()
        {
            var ip = Request.ServerVariables["REMOTE_ADDR"];
            var user = TopazUser.Active(ip);
            if (user == null) return PartialView("login");
            return PartialView("home");
        }
    }
}