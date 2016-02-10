using SiGotvime.Data.Models;
using SiGotvime.Data.Repository;
using SiGotvime.Data.ViewModels;
using SiGotvime.Utilities;
using SiGotvime__Web_.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    public class RecipeController : Controller
    {
        private IRecipeRepository _recipeRepository;
        private IIngredientRepository _ingredientRepository;
        private ITagRepository _tagRepository;

        public RecipeController(RecipeRepository recipeRepository,IngredientRepository ingredientRepository, TagRepository tagRepository)
        {
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
            _tagRepository = tagRepository;
        }

        public ActionResult GetRecipe(int id)
        {
            int adminUserID = Env.IsInRole(Constants.UserRoles.Administrator) ? Env.UserID() : 0;
            var recipe = _recipeRepository.GetCompleteRecipe(id,adminUserID);
            ViewBag.ImageUrl = recipe.ImageUrl;
            recipe.ImageUrl = CommonHelper.createImageUrl(recipe.ImageUrl);
            recipe.CroppedUrl = CommonHelper.createImageUrl(recipe.CroppedUrl);
            recipe.Comments = recipe.Comments.OrderBy(x => x.DateCreated).ToList();


            foreach(var user in recipe.UserLikes)
            {
                if(user.User!=null)
                {                    
                    user.User.ImageUrl = CommonHelper.createImageUrl(user.User.ImageUrl);
                }
            }

            foreach(var item in recipe.Comments)
            {
                if(item.User.ImageUrl.StartsWith("~"))
                    item.User.ImageUrl = CommonHelper.createImageUrl(item.User.ImageUrl);
            }
            var Ilike = _recipeRepository.UserLikesRecipe(recipe.ID, Env.UserID());
            ViewBag.Like = Ilike;
            return View(recipe);
        }

        public ActionResult Latest(int page=1)
        {
            var latestRecipes = _recipeRepository.GetLatestRecipes(page, Constants.PageSize);
            foreach(var item in latestRecipes.Recipes)
            {
                item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
            }
            return View(latestRecipes);
        }

        public ActionResult Search(int page=1)
        {
            SearchViewModel model = new SearchViewModel();
            model.Ingredients = _ingredientRepository.GetAll();
            model.Categories = _tagRepository.GetAll();
            model.SelectedIngredients = new List<Ingredient>();
            if(Session["selectedIngredients"] != null)
            {
                var selectedIngredients = (List<int>)Session["selectedIngredients"];
                model.result = _recipeRepository.SearchByIngredients(selectedIngredients, page, Constants.PageSize);
                model.SelectedIngredients = model.Ingredients.Where(x => selectedIngredients.Contains(x.ID)).ToList();
                foreach(var item in model.result.Recipes)
                {
                    item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                    item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
                }
                
            }
            model.Ingredients.ForEach(x=>{x.Recipes = null;});
            model.SelectedIngredients.ForEach(x => { x.Recipes = null; });
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(SearchModel model,int page=1)
        {
            int pageSize = Constants.PageSize;
            var recipes = _recipeRepository.SearchByIngredients(model.ingredients,page,pageSize);
            SearchViewModel vm = new SearchViewModel();
            vm.Ingredients = _ingredientRepository.GetAll();
            vm.Ingredients.ForEach(x => x.Recipes = null);
            vm.Categories = _tagRepository.GetAll();
            vm.result = recipes;
            vm.SelectedIngredients = vm.Ingredients.Where(x => model.ingredients.Contains(x.ID)).ToList();
            Session["selectedIngredients"] = model.ingredients;
            foreach(var item in vm.result.Recipes)
            {
                item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
            }
            return View(vm);
        }

        public ActionResult Category(int id,int page=1)
        {
            int categoryID = id;
            var result = _recipeRepository.GetRecipesByCategory(page, Constants.PageSize, categoryID);
            result.EntityID = categoryID;
            foreach(var item in result.Recipes)
            {
                item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
            }
            return View(result);
        }

        [Authorize]
        public ActionResult Like(int id)
        {
            int recipeID = id;
            int userID = Env.UserID();

            _recipeRepository.Like(recipeID, userID);
            return RedirectToAction("GetRecipe", new { id = recipeID });
        }

        public JsonResult GetRecipeLikes(int id)
        {
            var result = _recipeRepository.GetRecipeLikes(id);
            foreach(var item in result)
            {
                item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(int recipeID, string content)
        {
            int userID = Env.UserID();
            _recipeRepository.CreateComment(recipeID, userID, content);
            var comments = _recipeRepository.GetCommentsForRecipe(recipeID);
            comments.ForEach(x =>
            {
                x.User.ImageUrl = CommonHelper.createImageUrl(x.User.ImageUrl);
            });
            return PartialView("_CommentSection", comments);
        }
        
        [Authorize]
        public ActionResult Create()
        {
            RecipeModel model = new RecipeModel();
            model.Ingredients = _ingredientRepository.GetAll().Select(x => new IngredientViewModel { Name = x.Name, ID = x.ID }).ToList();
            model.Categories = _tagRepository.GetAll();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(RecipeModel model, PictureUploadModel pictureModel)
        {
            ImageHelper imageHelper = new ImageHelper { Width = 1500 };
            Image img = Image.FromStream(pictureModel.ImageToUpload.InputStream);

            ImageResult firstImage = imageHelper.RenameUploadFile(pictureModel.ImageToUpload);

            var cropped = imageHelper.cropImage(img, new Rectangle { X = (int)pictureModel.imgx, Y = (int)pictureModel.imgy, Width = (int)pictureModel.imgw, Height = (int)pictureModel.imgh });
            ImageResult croppedImgResult = imageHelper.SaveNewImage(cropped, Path.GetFileName(pictureModel.ImageToUpload.FileName));

            model.ImageUrl = String.Format("~/Content/images/{0}", firstImage.ImageName);
            model.CroppedImageUrl = String.Format("~/Content/images/{0}", croppedImgResult.ImageName);
            model.UserID = Env.UserID();
            model.isAdmin = Env.UserRoles().Contains(Constants.UserRoles.Administrator);
            int recipeID = _recipeRepository.CreateRecipe(model);

            TempData["RecipeSaved"] = true;

            return RedirectToAction("RecipeSaved");
        }

        public ActionResult RecipeSaved()
        {            
            if(TempData["RecipeSaved"]!=null)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}