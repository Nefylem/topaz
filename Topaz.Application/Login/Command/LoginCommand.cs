using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Topaz.Application.Common.Command;
using Topaz.Model;

namespace Topaz.Application.Login.Command
{
    class LoginCommand
    {
        private const string SessionKeySalt = "t0t3m54l7!";
        public string Check(string user, string password, string ip)
        {
            user = user.Trim().ToLower();
            var checkPass = new EncryptionControl().Encrypt(password);

            var database = new Entity();
            var record = database.users.FirstOrDefault(s => s.username == user);
            if (record == null) return "There was a problem accessing your account (wrong username or password)";

            if (!string.Equals(record.password, checkPass)) return "There was a problem accessing your account (wrong username or password)";
            //Todo: check for disabled accounts
            //"Your account has been disabled. Please contact support";

            CreateSession(record.user_id, record.username, ip);
            return "ok";
        }


        //Creates an identity to store on the browser and database for comparison
        private void CreateSession(int userId, string username, string ip)
        {
            var session = new HttpCookie("SystemSession") { Expires = DateTime.Now.AddHours(12) };
            var key = GetSessionKey(username, DateTime.Now);
            session["UserId"] = userId.ToString();
            session["SessionKey"] = key;
            session["SessionStart"] = DateTime.Now.ToShortDateString();
            HttpContext.Current.Response.Cookies.Add(session);

            var database = new Entity();
            database.user_session.Add(new user_session()
            {
                user_id = userId,
                user_ip = ip,
                expiry = DateTime.Now.AddHours(1),
                session = key
            });
            try
            {
                database.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //todo: log this error - there should not be an issue saving a session key
            }
        }

        public string GetSessionKey(string username, DateTime date)
        {
            var key = $"{username}{date.ToShortDateString()}{date.ToShortTimeString()}{SessionKeySalt}";
            return ToMd5Hash(key);
        }

        public string ToMd5Hash(string str)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(str);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
