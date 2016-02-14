using System.Linq;
using System.Web;

namespace SimpleStudentsWebsite.Classes.Helpers
{
    public class CookieHelper
    {
        private static readonly string RoleKey = "SSWRole";

        private static CookieHelper mInstance = null;
        public static CookieHelper Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new CookieHelper();
                return mInstance;
            }
        }

        public static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        public string GetValue(string name)
        {
            HttpCookie ck = null;
            if (Context == null) return null;
            if (Context.Request.Cookies.AllKeys.Contains(name))
                ck = Context.Request.Cookies.Get(name);
            if (Context.Response.Cookies.AllKeys.Contains(name))
            {
                var updck = Context.Response.Cookies.Get(name);
                if (updck != null && !string.IsNullOrEmpty(updck.Value))
                    ck = updck;
            }
            if (ck == null) return null;
            string value = ck.Value;
            if (!string.IsNullOrEmpty(value))
                value = AESCrypt.DescryptString(value, name);
            return value;
        }

        public void SetValue(string name, string value)
        {
            if (value == null)
                RemoveValue(name);
            else
            {
                string enc = AESCrypt.EncryptString(value, name);
                SetValue(new HttpCookie(name, enc));
            }
        }

        private void SetValue(HttpCookie ck)
        {
            ck.HttpOnly = true;
            Context.Response.Cookies.Set(ck);
        }

        private void RemoveValue(string name)
        {
            Context.Response.Cookies.Remove(name);
            Context.Request.Cookies.Remove(name);
        }


        public Roles Role
        {
            get
            {
                Roles result = Roles.Guest;
                var role = GetValue(RoleKey);
                if (string.IsNullOrEmpty(role))
                    return result;
                return EnumHelper<Roles>.Parse(role);
            }
            set
            {
                SetValue(RoleKey, "" + value.ToString("D"));
            }
        }
    }
}