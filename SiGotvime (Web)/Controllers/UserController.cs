using SiGotvime.Data.Models;
using SiGotvime.Data.Repository;
using SiGotvime.Data.ViewModels;
using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: User
        public ActionResult Profile(int id=0)
        {
            if(id == 0)
                id = Env.UserID();
            if(TempData["Reload"] != null)
                ViewBag.Reload = (bool)TempData["Reload"];

            var user = _userRepository.GetProfileInfo(id);
            user.ImageUrl = CommonHelper.createImageUrl(user.ImageUrl);
            return View(user);
        }
        
        public JsonResult EditProfileData(string name, string value, int userID = 0)
        {
            if(userID==0)
                userID = Env.UserID();
            _userRepository.UpdateUserProperty(userID, name, value);
            return Json(true);
        }

        [HttpPost]
        public ActionResult UploadPicture(int id,PictureUploadModel model)
        {
            int userID = 0;
            if(id == 0)
                userID = Env.UserID();
            else userID = id;

            var user = _userRepository.GetUserById(userID);
            if(user!=null && model.ImageToUpload!=null)
            {
                if(user.ImageUrl != null && user.ImageUrl.Contains("userImages") && System.IO.File.Exists(Server.MapPath(user.ImageUrl)))
                {
                    System.IO.File.Delete(Server.MapPath(user.ImageUrl));
                }

                string extension = Path.GetExtension(model.ImageToUpload.FileName);
                string imageTitle = string.Format("picture-{0}{1}", user.Username, extension);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/userImages"), imageTitle);
                var img = Image.FromStream(model.ImageToUpload.InputStream, true, true);
                var crop = cropImage(img, new RectangleF { X = model.imgx, Y = model.imgy, Width = model.imgw, Height = model.imgh });
                user.ImageUrl = String.Format("~/Content/userImages/{0}", imageTitle);
                crop.Save(path);

                _userRepository.UpdateUser(user);
                user.ImageUrl = CommonHelper.createImageUrl(user.ImageUrl);
                CommonHelper.CreateCookie(user);
            }

            TempData["Reload"] = true;
            if(id==0)
                return RedirectToAction("Profile");
            else return RedirectToAction("Profile", new { id = id });
        }


        private static Image cropImage(Image img, RectangleF cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        public PartialViewResult UserRecipes(int page = 1)
        {
            int userID = Env.UserID();

            List<Recipe> recipes = _userRepository.GetUserRecipes(userID);
            recipes.ForEach(x => x.CroppedUrl = CommonHelper.createImageUrl(x.CroppedUrl));
            return PartialView("_UserRecipes",recipes);
        }

        public PartialViewResult UserFavourites(int page = 1)
        {
            int userID = Env.UserID();
            var recipes = _userRepository.GetUserFavouriteRecipes(userID);
            recipes.ForEach(x => x.CroppedUrl = CommonHelper.createImageUrl(x.CroppedUrl));
            return PartialView("_UserFavouriteRecipes",recipes);
        }

        public PartialViewResult UserBlogPosts(int page=1)
        {
            var blogposts = _userRepository.GetUserBlogPosts(Env.UserID());            
            return PartialView("_UserBlogPosts", blogposts);
        }

    }
}