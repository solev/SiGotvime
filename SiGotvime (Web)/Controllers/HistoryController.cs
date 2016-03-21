using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    public class HistoryController : Controller
    {
        // GET: History
        public ActionResult FeaturedUsers()
        {

            return View();
        }

        public ActionResult RecipesOfTheDay()
        {
            return View();
        }
    }
}