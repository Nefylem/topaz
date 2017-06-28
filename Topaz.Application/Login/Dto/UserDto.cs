using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topaz.Application.Login.Dto
{
    public class UserDto
    {
        public class NewUser
        {
            public string Given { get; set; }
            public string Surname { get; set; }
            public string Other { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string Password { get; set; }
        }
    }
}
