using SiGotvime.Data.Repository;
using SiGotvime.Data.Models;
using SiGotvime.Data.ViewModels;
using SiGotvime.Data;
using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;

namespace Food_App_Service.Controllers
{
    [RoutePrefix("api/Recipe")]
    public class RecipeController : ApiController
    {        
        IRecipeRepository recipeRepository;
        FoodDatabase db;
        public RecipeController(RecipeRepository _recipeRepository, FoodDatabase _db)
        {
            recipeRepository = _recipeRepository;
            db = _db;
        }

        [Route("search")]
        [HttpPost]
        public IEnumerable<RecipeViewModel> search(SearchModel model)
        {
            
            Dictionary<Recipe, int> recipes = new Dictionary<Recipe, int>();
            List<RecipeViewModel> result = new List<RecipeViewModel>();
            
            foreach(var t in model.ingredients)
            {
                var tempList = db.IngredientsInRecipe.Where(x=> x.Ingredient.ID == t).Select(x=>x.Recipe).ToList();
                foreach(var r in tempList)
                {
                    if(!recipes.ContainsKey(r))
                    {
                        recipes.Add(r, 1);
                    }
                    else
                    {
                        recipes[r]++;
                    }
                }
            }

            int take = 5;
            if(model.taken == 0)
                take = 15;


            var sortedRecipes = recipes.OrderByDescending(x => x.Value).Skip(model.taken).Take(take).ToList();

            foreach(var item in sortedRecipes)
            {
                RecipeViewModel vm = new RecipeViewModel { Content = item.Key.Content, ImageUrl = CommonHelper.createImageUrl(item.Key.ImageUrl), CroppedUrl = item.Key.CroppedUrl != null ? CommonHelper.createImageUrl(item.Key.CroppedUrl) : null, PreparationTime = item.Key.PreparationTime, Title = item.Key.Title, Rating = item.Key.Rating };
                var list = db.IngredientsInRecipe.Where(x => x.Recipe.ID == item.Key.ID).ToList();
                List<IngredientsRecipe> ordered = new List<IngredientsRecipe>();

                foreach(var ing in list)
                {
                    var t = model.ingredients.FirstOrDefault(x => x == ing.Ingredient.ID);
                    if(t!=null)
                    {
                        ordered.Add(ing);
                    }
                }

                list.RemoveAll(x => ordered.Contains(x));
                ordered.AddRange(list);
                List<IngredientViewModel> tempList = new List<IngredientViewModel>();

                foreach(var ing in ordered)
                {
                    tempList.Add(new IngredientViewModel { Name = ing.Ingredient.Name, Quantity = ing.Quantity });
                }
                vm.ingredients = tempList;
                result.Add(vm);
                
            } 
            return result;
        }            
               

        [Route("GetByCategory")]
        [HttpPost]
        public IEnumerable<RecipeViewModel> GetByCategory(CategorySearchModel model)
        {
            var res = recipeRepository.GetByCategory(model.Categories[0], model.taken / 15);

            foreach(var item in res)
            {
                item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                if(item.CroppedUrl != null)
                {
                    item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
                }                
            }
            return res;
        }

        [Route("GetHomeRecipes")]
        [HttpPost]
        public IEnumerable<RecipeViewModel> getThem([FromBody] int taken)
        {
            string baseUrl = Constants.rootUrl;
            var recipes = recipeRepository.GetLatest((taken / Constants.RecipesPerPage) + 1);
            
            List<RecipeViewModel> res = new List<RecipeViewModel>();
            foreach(var item in recipes)
            {
                var r = recipeRepository.GetFullRecipeById(item);
                r.ID = item;
                r.ImageUrl = CommonHelper.createImageUrl(r.ImageUrl);
                if(r.CroppedUrl != null)
                {
                    r.CroppedUrl = CommonHelper.createImageUrl(r.CroppedUrl);
                }
                res.Add(r);
            }
            return res;
        }

        [Route("GetEasiest")]
        [HttpPost]
        public IEnumerable<RecipeViewModel> getEeasy([FromBody]int taken)
        {
            var result = recipeRepository.GetEasiest((taken/Constants.RecipesPerPage) +1);
            foreach(var item in result)
            {
                item.ImageUrl = CommonHelper.createImageUrl(item.ImageUrl);
                if(item.CroppedUrl!=null)
                {
                    item.CroppedUrl = CommonHelper.createImageUrl(item.CroppedUrl);
                }
            }
            return result;
        }
          

    }
}
