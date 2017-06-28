using System;
using System.Data.Entity.Validation;
using System.Linq;
using Topaz.Application.Common.Command;
using Topaz.Application.Common.Dto;
using Topaz.Application.Login.Dto;
using Topaz.Model;

namespace Topaz.Application.Login.Command
{
    class NewUserCommand
    {
        public CommonDto.CommandResult CreateNewUser(UserDto.NewUser data)
        {
            var database = new Entity();
            var username = (Get(data.Given) + "." + Get(data.Surname)).ToLower();
            var record = database.users.FirstOrDefault(s => s.username == username);
            if (record != null) return new CommonDto.CommandResult()
            {
                Success = false,
                Result = $"User {username} already exists. Please contact IT support if you are experiencing problems"
            };

            var password = new EncryptionControl().Encrypt(data.Password);
            database.users.Add(new user()
            {
                username = username,
                given = Get(data.Given),
                surname = Get(data.Surname),
                email = Get(data.Email),
                mobile = Get(data.Mobile),
                password = password
            });

            try
            {
                database.SaveChanges();
                //Todo: Use exchange to confirm the email address instead of doing this
                return new CommonDto.CommandResult()
                {
                    Result = $"Thank you. You can sign in using \"{username}\" and your given password",
                    Success = true
                };
            }
            catch (DbEntityValidationException ex)
            {
                var validation = ex.EntityValidationErrors.Aggregate("", (current, message) => current + message.Entry.ToString());
                return new CommonDto.CommandResult()
                {
                    Result = $"There was an error creating your account. Error message(s) {validation}",
                    Success = false
                };
            }
            catch (Exception ex)
            {
                return new CommonDto.CommandResult()
                {
                    Result = $"There was an error creating your account. Unknown error: {ex.Message}",
                    Success = false
                };
            }
        }

        public string Get(string field)
        {
            return !string.IsNullOrEmpty(field) ? field.Trim() : "";
        }
    }
}
