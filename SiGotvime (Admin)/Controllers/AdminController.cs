using SiGotvime.Data.Repository;
using SiGotvime.Data.Models;
using SiGotvime.Data.ViewModels;
using Food_App_Service.Properties;
using SiGotvime.Utilities;
using SiGotvime.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using Food_App_Service.Helpers;
using System.Net;
using System.Data.Entity;

namespace Food_App_Service.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IIngredientRepository ingredientRepository;
        IRecipeRepository recipeRepository;

        FoodDatabase db;

        public AdminController(IngredientRepository _ingredientRepository, RecipeRepository _recipeRepository, FoodDatabase _db)
        {
            ingredientRepository = _ingredientRepository;
            recipeRepository = _recipeRepository;
            db = _db;
        }
        public ActionResult Video()
        {
            var files = System.IO.Directory.GetFiles(Server.MapPath("~/Content/video"));
            if(files.Length > 0)
            {
                string filename = Path.GetFileName(files[0]);
                ViewBag.src = filename;
            }
            return View();
        }

        [HttpPost]
        public ActionResult UploadVideo(HttpPostedFileBase video)
        {

            var files = System.IO.Directory.GetFiles(Server.MapPath("~/Content/video"));
            foreach(var item in files)
            {
                if(System.IO.File.Exists(item))
                {
                    System.IO.File.Delete(item);
                }
            }

            string name = System.IO.Path.GetFileName(video.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~/Content/video"), name);
            video.SaveAs(path);
            
            return RedirectToAction("Video");
        }

        public ActionResult Statuses()
        {
            var list = db.Statuses.ToList();
            return View(list);
        }
        
        [HttpPost]
        public JsonResult AddStatus(Status model)
        {
            if(ModelState.IsValid)
            {
                db.Statuses.Add(model);
                db.SaveChanges();
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult DeleteStatus(Status model)
        {
            var status = db.Statuses.FirstOrDefault(x => x.ID == model.ID);
            if(status != null)
            {
                db.Statuses.Remove(status);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
                      
        }

        [HttpPost]
        public JsonResult ChangeSelectedStatuses(List<int> list)
        {
            var oldSelected = db.Statuses.Where(x => x.Selected);
            foreach(var item in oldSelected)
            {
                item.Selected = false;
            }          
 
           if(list != null)
           {
               foreach(var item in list)
               {
                   var tempStatus = db.Statuses.FirstOrDefault(x => x.ID == item);
                   tempStatus.Selected = true;
               }
           }
           db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

                
        public ActionResult Ingredients()
        {
            var list = ingredientRepository.GetAll();
            return View(list);
        }
                

        public ActionResult Recipes(int page = 1,string search = "")
        {

            RecipeListingModel result = new RecipeListingModel();
            if (page < 1)
                page = 1;
            if(search=="")
            {
                result = recipeRepository.GetAll(page);
            }
            else
            {
                result = recipeRepository.SearchRecipes(search, page);
            }

            int count = result.Count;
            ViewBag.RecipeCount = count/15;
            if(count % 15 > 0)
                ViewBag.RecipeCount++;
            foreach(var item in result.Recipes)
            {
                if(item.ImageUrl!=null)
                    item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                else
                {
                    item.ImageUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAHgAAAB4CAYAAAA5ZDbSAAACG0lEQVR4nO3XMXKDMBQA0dz/KHTqaOjoVKrnCLoCqT4jsJI0YSzWW2xhy7Y8evMx/qq17sbt691fwAQ2gU3gD01geALDExiewPAEhicwPIHhCQxPYHgCwxMYnsDwBIYnMDyB4QkMT2B4AsMTGJ7A8ASGJzA8geEJDE9geALDExiewPAEhicwPIHhCQxPYHgCwxMYnsDwBIYnMDyB4QkMT2B4AsMTGJ7A8ASGJzA8geEJDE9geEMDz/O8T9N0ei7nvE/TdJRzPta2bTutbds29H4fDdwe3PVAl2XZa637uq6ng00pHWvzPO8ppWH3+1jgdipSSi8T1RbTlXPeSymnCYu1UsqxFhjLshxrd+z37jMcHjgmoXfJbIv1UsrLAbcYLWpMYWDftd8oDQfcO9DeWoAFzvXArxNWa/8yfOd+I/RI4Djs3gT9NlHxvpjeu/cboccBxyX2+nv3129irPfee8d+o/Qo4PaGqDcp7XRe72rjBio+o3fH+5/7jdKjgK//Sa+H/9P/0pjCeF08Xtf1lv1GamhgE9gE/uwEhicwPIHhCQxPYHgCwxMYnsDwBIYnMDyB4QkMT2B4AsMTGJ7A8ASGJzA8geEJDE9geALDExiewPAEhicwPIHhCQxPYHgCwxMYnsDwBIYnMDyB4QkMT2B4AsMTGJ7A8ASGJzA8geEJDE9geALDExiewPAEhicwPIHhCQxPYHgCw/sGNlhdDky4v4YAAAAASUVORK5CYII=";
                }
                if(item.CroppedUrl!=null)
                    item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
            }
            return View(result.Recipes);
        }

        public ActionResult AddIngredient()
        {
            return View();
        }       


        [HttpPost]
        public ActionResult AddIngredient(Ingredient model)
        {
            if(model.Name==null || model.Name.Trim() == "")
            {
                ViewBag.error = "Името не смее да е празно!";
                return View(model);
            }

            if(ModelState.IsValid)
            {
                var temp = ingredientRepository.GetByName(model.Name);
                if(temp != null )
                {
                    ViewBag.error = "Таа состојка веќе постои!";
                    return View(model);
                }
                bool res = ingredientRepository.Add(model);
                if(res)
                    return RedirectToAction("Ingredients");
            }
            return View(model);
        }

        public ActionResult DeleteIngredient(int id)
        {
            var res = ingredientRepository.Delete(id);
            return RedirectToAction("Ingredients");
        }
        public ActionResult EditIngredient(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ingredient ing = db.Ingredients.Find(id);
            return View(ing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIngredient(Ingredient model)
        {
            if (model.Name == null || model.Name.Trim() == "")
            {
                ViewBag.error = "Името не смее да е празно!";
                return View(model);
            }           

            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Ingredients");
            }
            return View(model);
        }

        public ActionResult AddRecipe()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddRecipe(RecipeViewModel model,int x, int y, int width, int height)
        {
            if(ModelState.IsValid)
            {
                ImageHelper imageHelper = new ImageHelper { Width = 1600 };
                var ImageFromFile = Image.FromStream(model.ImageToUpload.InputStream, true, true);
                var croppedImage = imageHelper.cropImage(ImageFromFile, new Rectangle { X = x, Y = y, Height = height, Width = width });

                ImageResult firstImage = imageHelper.RenameUploadFile(model.ImageToUpload);
                if (firstImage.Success == false)
                {
                    TempData["Message"] = firstImage.ErrorMessage;
                    return RedirectToAction("AddRecipe");
                }


                ImageResult croppedImageResult = imageHelper.SaveNewImage(croppedImage, Path.GetFileName(model.ImageToUpload.FileName).Replace(" ", ""));
                if (croppedImageResult.Success == false)
                {
                    TempData["Message"] = croppedImageResult.ErrorMessage;
                    return RedirectToAction("AddRecipe");
                }
                model.ImageUrl = String.Format("~/Content/images/{0}", firstImage.ImageName);
                model.CroppedUrl = String.Format("~/Content/images/{0}", croppedImageResult.ImageName);  
                model.Approved = true;
                int userID = Env.UserID();
                var res = recipeRepository.Add(model,userID);
                if(res)
                    return RedirectToAction("Recipes");
            }

            ViewBag.Error = "Рецептот не се зачува. Пробајте повторно";
            return View(model);
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        ///// <summary>
        ///// Saves the image to  specified stream and format.
        ///// </summary>
        ///// <param name="image">The image to save.</param>
        ///// <param name="outputStream">The stream to used.</param>
        ///// <param name="format">The format of new image.</param>
        ///// <param name="quality">The quality of the image in percent.</param>
        //public static void SaveTo(this Image image, Stream outputStream, ImageFormat format, int quality)
        //{
        //    EncoderParameters encoderParameters = new EncoderParameters(1);
        //    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
        //    ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
        //    if (format == ImageFormat.Gif)
        //    {
        //        image.Save(outputStream, ImageFormat.Gif);
        //    }
        //    else if (format == ImageFormat.Jpeg)
        //    {
        //        image.Save(outputStream, encoders[1], encoderParameters);
        //    }
        //    else if (format == ImageFormat.Png)
        //    {
        //        image.Save(outputStream, encoders[4], encoderParameters);
        //    }
        //    else if (format == ImageFormat.Bmp)
        //    {
        //        image.Save(outputStream, encoders[0], encoderParameters);
        //    }
        //    else
        //    {
        //        image.Save(outputStream, format);
        //    }
        //}
          

        public ActionResult EditRecipe(int id)
        {
            int UserAdminID = Env.UserRoles().FirstOrDefault(x => x == 1);
            var recipe = recipeRepository.GetCompleteRecipe(id, UserAdminID);
            var recipeViewModel = new RecipeViewModel
            {
                Content = recipe.Content,
                ingredients = recipe.Ingredients.Select(x => new IngredientViewModel { Name = x.Ingredient.Name, Quantity = x.Quantity }),
                ImageUrl = CommonHelper.createImageUrl(recipe.ImageUrl),
                CroppedUrl = CommonHelper.createImageUrl(recipe.CroppedUrl),
                PreparationTime = recipe.PreparationTime,
                Rating = recipe.Rating,
                tags = recipe.Categories.Select(x=>x.Tag.ID),
                Title = recipe.Title,
                NumberOfPeople = recipe.NumberOfPeople,
                Difficulty = recipe.Difficulty
            };
            return View(recipeViewModel);
        }

        [HttpPost]
        public ActionResult EditRecipe(RecipeViewModel model, int recipeID, int x=0, int y=0, int width=0, int height=0,int page=1)
        {

            var recipe = recipeRepository.FindById(recipeID);
            ImageHelper imageHelper = new ImageHelper();
            imageHelper.Width = 1600;
            if(model.ImageToUpload != null && width!=0 && height!=0)
            {
                
                if(recipe.ImageUrl != null && System.IO.File.Exists(Server.MapPath(recipe.ImageUrl)))
                {
                    System.IO.File.Delete(Server.MapPath(recipe.ImageUrl));
                }
                if(recipe.CroppedUrl!=null && System.IO.File.Exists(Server.MapPath(recipe.CroppedUrl)))
                {
                    System.IO.File.Delete(Server.MapPath(recipe.CroppedUrl));
                }

                
                var ImageFromFile = Image.FromStream(model.ImageToUpload.InputStream, true, true);
                var croppedImage = imageHelper.cropImage(ImageFromFile, new Rectangle { X = x, Y = y, Height = height, Width = width });

                ImageResult firstImage = imageHelper.RenameUploadFile(model.ImageToUpload);
                if(firstImage.Success == false)
                {
                    TempData["Message"] = firstImage.ErrorMessage;
                    return RedirectToAction("EditRecipe", new { id = recipeID });
                }
                
                
                ImageResult croppedImageResult = imageHelper.SaveNewImage(croppedImage, Path.GetFileName(model.ImageToUpload.FileName).Replace(" ",""));
                if (croppedImageResult.Success == false)
                {
                    TempData["Message"] = croppedImageResult.ErrorMessage;
                    return RedirectToAction("EditRecipe", new { id = recipeID });
                }
                model.ImageUrl = String.Format("~/Content/images/{0}", firstImage.ImageName);
                model.CroppedUrl = String.Format("~/Content/images/{0}", croppedImageResult.ImageName);         
              }

            var res = recipeRepository.Edit(model,recipeID);

            return RedirectToAction("Recipes", new { page=page});           
            
        }

        public ActionResult DeleteRecipe(int id)
        {
            var recipe = recipeRepository.FindById(id);
            if(recipe!=null)
            {
                if(recipe.ImageUrl!=null && System.IO.File.Exists(Server.MapPath(recipe.ImageUrl)))
                {
                    System.IO.File.Delete(Server.MapPath(recipe.ImageUrl));
                }
                if(recipe.CroppedUrl!=null && System.IO.File.Exists(Server.MapPath(recipe.CroppedUrl)))
                {
                    System.IO.File.Delete(Server.MapPath(recipe.CroppedUrl));
                }
            }
            recipeRepository.Delete(id);
            return RedirectToAction("Recipes");
        }

        public ActionResult Categories()
        {
            var list = db.Tags;
            return View(list);
        }

        public ActionResult AddCategory()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult AddCategory(Tag model)
        {

            if(model.Name == null || model.Name.Trim() == "")
            {
                ViewBag.error = "Името не смее да е празно!";
                return View(model);
            }

            if(ModelState.IsValid)
            {
                var check = db.Tags.FirstOrDefault(x => x.Name == model.Name);

                if(check == null)
                {
                    db.Tags.Add(model);
                    var res = db.SaveChanges();
                    if(res > 0)
                        return RedirectToAction("Categories");
                }
                ViewBag.error = "Веќе постои таква категорија!";
                return View(model);
            }            
            return View(model);
        }
        public ActionResult EditCategory(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tag category = db.Tags.Find(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(Tag category)
        {
            if (category.Name == null || category.Name.Trim() == "")
            {
                ViewBag.error = "Името не смее да е празно!";
                return View(category);
            }           

            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Categories");
            }            
            return View(category);
        }

        public ActionResult DeleteCategory(int id)
        {
            var cat = db.Tags.FirstOrDefault(x => x.ID == id);
            if(cat!=null)
            {
                db.Tags.Remove(cat);
                db.SaveChanges();
            }
            return RedirectToAction("Categories");
        }

        public ActionResult RecipesOfTheDay()
        {
            int recipeOfTheDayID = Convert.ToInt32(db.Settings.FirstOrDefault(x => x.SettingKey == Constants.RecipeOfTheDay).SettingValue);
            var recipes = db.Recipes.Select(x => new{x.Title,x.ID}).ToList();
            ViewBag.RecipeID = recipeOfTheDayID;
            List<Recipe> result = recipes.Select(x => new Recipe { Title = x.Title, ID = x.ID }).ToList();
            return View(result);
        }

        public ActionResult RecipeOfTheDaySelect(int id)
        {
            Settings setting = db.Settings.FirstOrDefault(x => x.SettingKey == Constants.RecipeOfTheDay);
            setting.SettingValue = id.ToString();
            db.SaveChanges();

            return RedirectToAction("RecipesOfTheDay");
        }
	}
}