﻿using SiGotvime__Web_.Helpers;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CaptchaFilterAttribute());
        }
    }
}
