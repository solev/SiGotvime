using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString reCaptcha(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            string publickey = Constants.ReCaptchaSiteKey;
            sb.AppendLine("<div class=\"g-recaptcha\"data-sitekey=\"" + publickey + "\"></div>");
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}