using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topaz.Application.Common.Dto;
using Topaz.Application.Login.Dto;

namespace Topaz.Application.Login
{
    public interface ILoginService
    {
        CommonDto.CommandResult CreateNewUser(UserDto.NewUser newUser);
        string CheckLogin(string user, string password, string ip);
    }
}
