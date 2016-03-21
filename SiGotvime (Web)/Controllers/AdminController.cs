
using SiGotvime.Data.Repository;
using SiGotvime.Data.Result_Models;
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
        private readonly IUserRepository _userRepository;

        public AdminController(IRecipeRepository recipeRepository, IUserRepository userRepository)
        {
            _recipeRepository = recipeRepository;
            _userRepository = userRepository;
        }

        public ActionResult UnApprovedRecipes(int page = 1)
        {
            if(!Env.IsInRole(Constants.UserRoles.Administrator))
                throw new UnauthorizedAccessException();

            if(page<=0)
                page=1;
            var recipes = _recipeRepository.GetUnApproved(page, Constants.PageSize);
            return View(recipes);
        }

        public ActionResult ApproveRecipe(int id, bool approved)
        {
            if (!Env.IsInRole(Constants.UserRoles.Administrator))
                throw new UnauthorizedAccessException();

            _recipeRepository.ApproveRecipe(id, approved);
            return RedirectToAction("UnApprovedRecipes");
        }

        public ActionResult RegisteredUsers(int page = 1)
        {
            if (!Env.IsInRole(Constants.UserRoles.Administrator))
                throw new UnauthorizedAccessException();

            if(page < 1)
                page = 1;

            ListUsersResult model = _userRepository.GetAllUsers(page, Constants.PageSize);
            return View(model);
        }

    }
}