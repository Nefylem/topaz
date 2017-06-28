using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topaz.Application.Common.Dto;
using Topaz.Application.Login.Command;
using Topaz.Application.Login.Dto;

namespace Topaz.Application.Login
{
    public class LoginService : ILoginService
    {
        public CommonDto.CommandResult CreateNewUser(UserDto.NewUser newUser)
        {
            return new NewUserCommand().CreateNewUser(newUser);
        }
    }
}
