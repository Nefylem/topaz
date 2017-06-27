using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topaz.Application.Login;

namespace Topaz.Application
{
    public class Provider
    {
        public ILoginService Login { get; private set; }
        public Provider()
        {
            Login = new LoginService();
        }
    }
}
