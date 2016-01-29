using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SiGotvime.Utilities
{
    public class CommonHelper
    {        
        public static string ShortenContent(string content,int maxLenght = 250)
        {
            if (content.Length <= maxLenght)
                return content;

            return content.Substring(0, maxLenght);
        }
        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if(!string.IsNullOrWhiteSpace(appUrl) && appUrl.Replace("/","")!="") appUrl += "/";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }
        
        public static string createImageUrl(string imageUrl)
        {
            if(imageUrl == null) return "";
            if(imageUrl.StartsWith("https"))
                return imageUrl;
            string baseUrl = GetBaseUrl();
            return string.Format("{0}{1}", baseUrl, imageUrl.Replace("~/", ""));
        }

        public static void CreateCookie(User user)
        {
            string userData = string.Concat(user.ID, "|", user.Username, "|", user.FirstName, "|", user.LastName, "|", user.ImageUrl, "|", string.Join(",",user.Roles.Select(x => x.ID).ToList()));
            var tkt = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(60), false, userData);
            var cookiestr = FormsAuthentication.Encrypt(tkt);
            var ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            ck.Path = FormsAuthentication.FormsCookiePath;
            HttpContext.Current.Response.Cookies.Add(ck);
        }
        public static void CreateCookieFacebook(FacebookUser fbUser)
        {
            var user = fbUser.User;
            string userData = string.Concat(user.ID, "|", user.Username, "|", user.FirstName, "|", user.LastName, "|", user.ImageUrl, "|","",fbUser.Link);
            var tkt = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(60), false, userData);
            var cookiestr = FormsAuthentication.Encrypt(tkt);
            var ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            ck.Path = FormsAuthentication.FormsCookiePath;
            HttpContext.Current.Response.Cookies.Add(ck);
        }        

        public static RecipeDifficulty GetRecipeDifficulty(int time)
        {
            if(time < 30)
                return RecipeDifficulty.Easy;
            else if(time < 60)
                return RecipeDifficulty.Medium;
            else return RecipeDifficulty.Hard;

        }

        public static string EvaluateDifficulty(RecipeDifficulty difficulty)
        {
            switch(difficulty)
            {
                case RecipeDifficulty.Easy:
                    return "Лесно";
                case RecipeDifficulty.Medium:
                    return "Средно";
                case RecipeDifficulty.Hard:
                    return "Тешко";
                default:
                    return "Средно";
            }
        }
        public static string EvaluateDifficultyClass(RecipeDifficulty difficulty)
        {
            switch(difficulty)
            {
                case RecipeDifficulty.Easy:
                    return "i-easy";
                case RecipeDifficulty.Medium:
                    return "i-medium";
                case RecipeDifficulty.Hard:
                    return "i-hard";
                default:
                    return "i-medium";
            }
        }

    }
}