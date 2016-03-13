using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Utilities
{
    public class Constants
    {
        public static int RecipesPerPage = 15;
        //public static string rootUrl = "http://portal-baranjerecepti.azurewebsites.net";
        public static string rootUrl = "http://localhost:61820";

        public static string RecipeOfTheDay = "RecipeOfTheDay";
        public static string FeaturedMember = "FeaturedMember";

        public static int HomePageSize = 6;
        public static int PageSize = 15;
        public static string DefaultImgUrl = "~/images/avatar.jpg";

        public static string ReCaptchaSecretKey = "6LdxrRoTAAAAAHFQanrJHbzuwHZPDF4ITcJYnYIR";
        public static string ReCaptchaSiteKey = "6LdxrRoTAAAAAGJQ0EmTgdg9AZGns0yh-Wq4KSBd";

        public static class UserRoles
        {
            //Database ID's for the roles

            public static int Administrator = 1;
        }
    }
}