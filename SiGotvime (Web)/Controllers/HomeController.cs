using SiGotvime.Data.Models;
using SiGotvime.Data.Repository;
using SiGotvime.Data.ViewModels;
using SiGotvime.Utilities;
using SiGotvime__Web_.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    //[CompressFilter]
    public class HomeController : Controller
    {
        private IRecipeRepository _recipeRepository;
        private IHomeRepository _homeRepository;
        private ISettingsRepository _settingsRepositoy;
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;
        private IBlogRepository _blogRepository;

        public HomeController(RecipeRepository recipeRepository, HomeRepository homeRepository, SettingsRepository settingsRepository, UserRepository userRepository, MessageRepository messageRepository, BlogRepository blogRepository)
        {
            _recipeRepository = recipeRepository;
            _homeRepository = homeRepository;
            _settingsRepositoy = settingsRepository;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _blogRepository = blogRepository;
        }

        public ActionResult Index()
        {
            HomePageViewModel model = _homeRepository.GetHomePageData();
            var result = _recipeRepository.GetLatestRecipes(1, 6);

            //photos
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Content/images"));
            model.Photos = directory.GetFiles().Length;
            directory = new DirectoryInfo(Server.MapPath("~/Content/userImages"));
            model.Photos += directory.GetFiles().Length;

            //latest recipes
            model.LatestRecipes = result.Recipes;
            foreach (var item in model.LatestRecipes)
            {
                item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                if (item.CroppedUrl != null)
                    item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
            }

            //latest Blogs
            var blogposts = _blogRepository.AllPosts(1, 6);
            blogposts.BlogPosts.ForEach(x =>
            {
                x.ImageUrl = CommonHelper.createImageUrl(x.ImageUrl);
            });
            model.BlogPosts = blogposts.BlogPosts;


            List<string> keys = new List<string>() { Constants.RecipeOfTheDay, Constants.FeaturedMember };
            List<Settings> featuredData = _settingsRepositoy.GetSettingsInKeys(keys);

            int featuredUserID = Convert.ToInt32(featuredData.FirstOrDefault(x => x.SettingKey == Constants.FeaturedMember).SettingValue);

            int recipeOfTheDayID = Convert.ToInt32(featuredData.FirstOrDefault(x => x.SettingKey == Constants.RecipeOfTheDay).SettingValue);

            model.RecipeOfTheDay = _recipeRepository.GetRecipeForHome(recipeOfTheDayID);
            model.RecipeOfTheDay.ImageUrl = CommonHelper.createImageUrl(model.RecipeOfTheDay.ImageUrl);


            model.FeaturedMember = _userRepository.GetFeaturedUser(featuredUserID);
            model.FeaturedMember.ImageUrl = CommonHelper.createImageUrl(model.FeaturedMember.ImageUrl);

            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult Advertisments()
        {
            return PartialView("Advertisments");
        }

        public ActionResult Contact()
        {
            if (TempData["MessageCreated"] != null)
            {
                ViewBag.Message = "Успешно е пратена пораката!";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(Message message)
        {
            if (!string.IsNullOrEmpty(message.Validate))
                return RedirectToAction("Contact");

            message.DateCreated = DateTime.Now;
            if (Env.UserID() != 0)
            {
                message.UserBy = new User { ID = Env.UserID() };
            }
            _messageRepository.CreateMessage(message);
            TempData["MessageCreated"] = true;
            return RedirectToAction("Contact");
        }

    }
}