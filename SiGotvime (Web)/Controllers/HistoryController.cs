using SiGotvime.Data.Repository;
using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHistoryRepository _historyRepository;
        public HistoryController(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }
        // GET: History
        public ActionResult FeaturedUsers()
        {

            return View();
        }

        public ActionResult RecipesOfTheDay(int page = 1)
        {
            if(page<1)
                page = 1;
            var model = _historyRepository.GetPreviousRecipesOfTheDay(page, Constants.PageSize);
            foreach (var item in model.Recipes)
            {
                item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
            }
            return View(model);
        }

        public ActionResult CreateInitialRecipeOfTheDay(int id)
        {
            _historyRepository.InsertRecipeOfTheDay(id);
            return RedirectToAction("RecipesOfTheDay");
        }
    }
}