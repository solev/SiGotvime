using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Utilities
{
    public class Constants
    {
        public const int RecipesPerPage = 15;
        //public static string rootUrl = "http://portal-baranjerecepti.azurewebsites.net";
        public const string rootUrl = "http://localhost:61820";

        public const string RecipeOfTheDay = "RecipeOfTheDay";
        public const string FeaturedMember = "FeaturedMember";

        public const int HomePageSize = 6;
        public const int PageSize = 15;
        public const string DefaultImgUrl = "~/images/avatar.jpg";

        public const string ReCaptchaSecretKey = "6LdxrRoTAAAAAHFQanrJHbzuwHZPDF4ITcJYnYIR";
        public const string ReCaptchaSiteKey = "6LdxrRoTAAAAAGJQ0EmTgdg9AZGns0yh-Wq4KSBd";

        public static class UserRoles
        {
            //Database ID's for the roles

            public const int Administrator = 1;
        }
    }
}