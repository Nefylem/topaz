using System.Web.Mvc;

namespace Topaz.Application
{
    public class TopazController : Controller
    {
        protected readonly Provider Topaz = new Provider();
    }
}
