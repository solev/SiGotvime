
using SiGotvime.Data.Repository;
using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private readonly IRecipeRepository _recipeRepository;

        public AdminController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public ActionResult UnApprovedRecipes(int page = 1)
        {
            if(!Env.IsInRole(Constants.UserRoles.Administrator))
                return RedirectToAction("Index","Home");

            if(page<=0)
                page=1;
            var recipes = _recipeRepository.GetUnApproved(page, Constants.PageSize);
            return View(recipes);
        }

        public ActionResult ApproveRecipe(int id,bool approved)
        {
            if (!Env.IsInRole(Constants.UserRoles.Administrator))
                return RedirectToAction("Index", "Home");

            _recipeRepository.ApproveRecipe(id, approved);
            return RedirectToAction("UnApprovedRecipes");
        }
    }
}