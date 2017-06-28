using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Topaz.Model;

namespace Topaz.Application
{
    public class TopazUser
    {
        public static TopazUser Active(string ip)
        {
            return GetActiveUser(ip);
        }

        public int Id;
        public string Username;
        public string Given;
        public string Surname;
        public string Email;
        public int AccessLevel;

        private static TopazUser GetActiveUser(string ip)
        {
            var userId = IsValidSession(ip);
            if (userId == -1) return null;
            return new TopazUser(userId);
        }

        public TopazUser(int userId)
        {
            //Todo: cache this for 10 minutes or until expiry
            var database = new Entity();
            var record = database.users.FirstOrDefault(s => s.user_id == userId);
            if (record == null) return;

            Id = record.user_id;
            Username = record.username;
            Given = record.given;
            Surname = record.surname;
            Email = record.email;
            AccessLevel = 10;           //Temporary, lookup permissions 
        }

        private static int IsValidSession(string ip)
        {
            var cookie = GetSessionCookie();
            if (cookie == null) return -1;

            var key = cookie.Values["SessionKey"];
            var database = new Entity();

            //in case the db drops out. No point logging this, just return to log in instead of giving error
            try
            {
                var sessions = database.user_session.Where(a => a.session.Equals(key) && a.user_ip.Equals(ip)).ToList();
                var active = sessions.FirstOrDefault();
                return active?.user_id ?? -1;
            }
            catch
            {
                return -1;
            }
        }

        public static void LogOut(string ip)
        {
            var cookie = GetSessionCookie();
            if (cookie == null) return;

            var key = cookie.Values["SessionKey"];

            var database = new Entity();
            var sessions = database.user_session.Where(a => a.session.Equals(key) && a.user_ip.Equals(ip)).ToList();

            foreach (var item in sessions)
            {
                item.expiry = DateTime.Now.AddSeconds(-30);
            }
            database.SaveChanges();

            cookie.Expires = DateTime.Now.AddSeconds(-30);
            HttpContext.Current.Request.Cookies.Add(cookie);
        }

        private static HttpCookie GetSessionCookie()
        {
            try
            {
                return HttpContext.Current.Request.Cookies["SystemSession"];
            }
            catch
            {
                return null;
            }
        }



    }
}

