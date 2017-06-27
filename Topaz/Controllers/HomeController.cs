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
            return View();
        }
    }
}