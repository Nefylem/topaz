using System.Web;
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

        public string CheckLogin(string user, string password, string ip)
        {
            return new LoginCommand().Check(user, password, ip);
        }
    }
}
