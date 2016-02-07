using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace SiGotvime.Utilities
{
    public class Env
    {
        public static string UserName()
        {
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                string[] userDataPieces = fi.Ticket.UserData.Split('|');
                return userDataPieces[1];
            }
            else
            {
                return "";
            }
        }

        public static int UserID()
        {
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                string[] userDataPieces = fi.Ticket.UserData.Split('|');
                return Convert.ToInt32(userDataPieces[0]);
            }
            else
            {
                return 0;
            }
        }

        public static List<int> UserRoles()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                string[] userDataPieces = fi.Ticket.UserData.Split('|');
                string result = userDataPieces[5];
                if (!string.IsNullOrEmpty(result))
                    return result.Split(',').Select(int.Parse).ToList();
                return new List<int>();
            }
            else
            {
                return new List<int>();
            }
        }

        public static bool IsInRole(int roleID)
        {
            var roles = UserRoles();
            return roles.Contains(roleID);
        }

        public static string FirstName()
        {
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                string[] userDataPieces = fi.Ticket.UserData.Split('|');
                return userDataPieces[2];
            }
            else
            {
                return "";
            }
        }

        public static string LastName()
        {
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                string[] userDataPieces = fi.Ticket.UserData.Split('|');
                return userDataPieces[3];
            }
            else
            {
                return "";
            }
        }

        public static string UserImage()
        {
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                string[] userDataPieces = fi.Ticket.UserData.Split('|');
                return userDataPieces[4];
            }
            else
            {
                return "";
            }
        }

        public static string FbLink()
        {
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                string[] userDataPieces = fi.Ticket.UserData.Split('|');
                if(userDataPieces.Length > 6)
                    return userDataPieces[6];
                return "";
            }
            else
            {
                return "";
            }
        }
    }
}
