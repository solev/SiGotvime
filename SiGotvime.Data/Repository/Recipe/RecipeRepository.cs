using SiGotvime.Data.Models;
using SiGotvime.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiGotvime.Data.ViewModels;
using System.Data.Entity;
using SiGotvime.Data.Result_Models;
using System.Text;

namespace SiGotvime.Data.Repository
{
    public class RecipeRepository : IRecipeRepository
    {

        private FoodDatabase db;
        public RecipeRepository(FoodDatabase _db)
        {
            db = _db;
        }
        const int recipesPerPage = 15;

        public bool Add(RecipeViewModel model, int UserID = 0)
        {
            Recipe recipeToAdd = new Recipe
            {
                Content = model.Content,
                Title = model.Title,
                PreparationTime = model.PreparationTime,
                ImageUrl = model.ImageUrl,
                CroppedUrl = model.CroppedUrl,
                Approved = model.Approved,
                Difficulty = model.Difficulty,
                DateCreated = DateTime.Now,
                NumberOfPeople = model.NumberOfPeople
            };


            var ingredientNames = model.ingredients.Select(y => y.Name).ToList();
            var ingredientsToAdd = db.Ingredients.Where(x => ingredientNames.Contains(x.Name)).ToList();
            var ings = ingredientsToAdd.Select(x => new IngredientsRecipe { Ingredient = x, Quantity = model.ingredients.FirstOrDefault(y => y.Name == x.Name).Quantity }).ToList();
            recipeToAdd.Ingredients = ings;

            var tags = db.Tags.Where(x => model.tags.Contains(x.ID)).ToList();
            recipeToAdd.Categories = tags.Select(x => new RecipeTag { Tag = x }).ToList();


            if (UserID != 0)
            {
                var user = db.Users.FirstOrDefault(x => x.ID == UserID);
                recipeToAdd.User = user;
            }

            db.Recipes.Add(recipeToAdd);

            int res = db.SaveChanges();
            return res > 0;
        }

        public RecipeViewModel GetFullRecipeById(int id)
        {
            var recipe = FindById(id);
            RecipeViewModel result = new RecipeViewModel { Content = recipe.Content, ImageUrl = recipe.ImageUrl, PreparationTime = recipe.PreparationTime, Title = recipe.Title, Rating = recipe.Rating, CroppedUrl = recipe.CroppedUrl };
            var ingredients = db.IngredientsInRecipe.Where(x => x.Recipe.ID == id).ToList();
            var tags = db.RecipeTags.Where(x => x.Recipe.ID == id).Select(x => x.Tag.ID).ToList();
            List<IngredientViewModel> tempIngredients = new List<IngredientViewModel>();

            foreach (var item in ingredients)
            {
                tempIngredients.Add(new IngredientViewModel { Name = item.Ingredient.Name, Quantity = item.Quantity });
            }

            result.ingredients = tempIngredients;
            result.tags = tags;

            return result;
        }

        public Recipe FindById(int id)
        {
            return db.Recipes.FirstOrDefault(x => x.ID == id);
        }


        public RecipeListingModel GetAll(int page)
        {
            var tempResult = db.Recipes.OrderBy(x => x.ID);
            RecipeListingModel result = new RecipeListingModel();
            result.Count = tempResult.Count();
            var listResult = tempResult.Skip((page - 1) * recipesPerPage).Take(recipesPerPage).ToList();
            result.Recipes = listResult;

            return result;
        }

        public bool Edit(RecipeViewModel model, int id)
        {
            var recipe = db.Recipes.Include(x => x.Ingredients).Include(x => x.Categories).FirstOrDefault(x => x.ID == id);


            db.IngredientsInRecipe.RemoveRange(recipe.Ingredients);
            recipe.Ingredients.Clear();
            if (model.ingredients != null)
            {
                var ingNames = model.ingredients.Select(y => y.Name).ToList();
                var newIngredients = db.Ingredients.Where(x => ingNames.Contains(x.Name)).ToList();
                newIngredients.ForEach(x =>
                {
                    recipe.Ingredients.Add(new IngredientsRecipe { Ingredient = x, Recipe = recipe, Quantity = model.ingredients.FirstOrDefault(y => y.Name == x.Name).Quantity });
                });
            }


            db.RecipeTags.RemoveRange(recipe.Categories);
            recipe.Categories.Clear();
            var categories = db.Tags.Where(x => model.tags.Contains(x.ID)).ToList();
            categories.ForEach(x =>
            {
                recipe.Categories.Add(new RecipeTag { Recipe = recipe, Tag = x });
            });


            if (model.ImageUrl != null)
            {
                recipe.ImageUrl = model.ImageUrl;
                recipe.CroppedUrl = model.CroppedUrl;
            }

            recipe.PreparationTime = model.PreparationTime;
            recipe.Title = model.Title;
            recipe.Content = model.Content;
            recipe.NumberOfPeople = model.NumberOfPeople;
            recipe.Difficulty = model.Difficulty;
            var res = db.SaveChanges();

            return res > 0;
        }


        public bool Delete(int id)
        {
            var recipe = db.Recipes.FirstOrDefault(x => x.ID == id);

            if (recipe != null)
            {

                var tags = db.RecipeTags.Where(x => x.Recipe.ID == recipe.ID);
                db.RecipeTags.RemoveRange(tags);

                var ings = db.IngredientsInRecipe.Where(x => x.Recipe.ID == id);
                db.IngredientsInRecipe.RemoveRange(ings);

                db.Recipes.Remove(recipe);
                var res = db.SaveChanges();

                return res > 0;
            }



            return false;

        }

        public List<int> GetLatest(int page)
        {

            var result = db.Recipes.Where(x => x.Approved).OrderByDescending(x => x.DateCreated).Skip((page - 1) * recipesPerPage).Take(recipesPerPage).Select(x => x.ID).ToList();
            return result;
        }

        public IEnumerable<RecipeViewModel> GetByCategory(int categoryID, int page = 1)
        {
            if (page == 0) page = 1;
            var tempRecipes = db.RecipeTags.Where(x => x.Tag.ID == categoryID).OrderBy(x => x.ID).Skip((page - 1) * recipesPerPage).Take(recipesPerPage).Select(x => x.Recipe.ID).ToList();
            List<RecipeViewModel> res = new List<RecipeViewModel>();
            foreach (var item in tempRecipes)
            {
                var recipe = GetFullRecipeById(item);
                res.Add(recipe);
            }

            return res;
        }


        public IEnumerable<RecipeViewModel> GetEasiest(int page)
        {
            if (page == 0) page = 1;
            var tempResult = db.Recipes.OrderBy(x => x.PreparationTime).Skip((page - 1) * recipesPerPage).Take(recipesPerPage).Select(x => x.ID).ToList();
            List<RecipeViewModel> result = new List<RecipeViewModel>();
            foreach (var item in tempResult)
            {
                var rec = GetFullRecipeById(item);
                result.Add(rec);
            }
            return result;
        }


        public RecipeListingModel SearchRecipes(string search, int page = 1)
        {
            var tempresult = db.Recipes.Where(x => x.Title.ToLower().Contains(search.ToLower())).OrderBy(x => x.ID);
            RecipeListingModel result = new RecipeListingModel();
            result.Count = tempresult.Count();
            var listResult = tempresult.Skip((page - 1) * recipesPerPage).Take(recipesPerPage).ToList();
            result.Recipes = listResult;

            return result;
        }

        public string GetRecipeImageUrl(int RecipeID)
        {
            return db.Recipes.Where(x => x.ID == RecipeID).Select(x => x.CroppedUrl ?? x.ImageUrl).FirstOrDefault();
        }


        public Recipe GetCompleteRecipe(int recipeID, int AdminUserID = 0)
        {
            var recipe = db.Recipes.Where(x => x.ID == recipeID && (x.Approved || AdminUserID != 0))
                .Include(x => x.Ingredients)
                .Include(x => x.Ingredients.Select(y => y.Ingredient))
                .Include(x => x.Categories)
                .Include(x => x.Categories.Select(y => y.Tag))
                .Include(x => x.User)
                .Include(x => x.Comments)
                .Include(x => x.Comments.Select(y => y.User)).FirstOrDefault();

            if (recipe != null)
                recipe.UserLikes = db.UserRecipes.Where(x => x.Recipe.ID == recipeID).Include(x => x.User).OrderBy(x => x.ID).Take(9).ToList();

            return recipe;
        }

        public void Dispose()
        {
            db.Dispose();
        }


        public RecipesResultModel GetLatestRecipes(int page, int pageSize)
        {
            var result = db.Recipes.Where(x => x.Approved).OrderByDescending(x => x.DateCreated).Select(x => new
            {
                x.ID,
                x.ImageUrl,
                x.CroppedUrl,
                x.Title,
                x.Difficulty,
                LikeCount = x.UserLikes.Count(),
                CommentCount = x.Comments.Count()
            });

            RecipesResultModel model = new RecipesResultModel();
            model.MaxCount = result.Count();
            model.Recipes = result.Skip((page - 1) * pageSize).Take(pageSize).ToList().Select(x =>
                new Recipe
                {
                    ID = x.ID,
                    ImageUrl = x.ImageUrl,
                    CroppedUrl = x.CroppedUrl,
                    Title = x.Title,
                    Difficulty = x.Difficulty,
                    CommentCount = x.CommentCount,
                    LikeCount = x.LikeCount
                }).ToList();

            return model;
        }


        public RecipeByCategoryResultModel GetRecipesByCategory(int page, int pageSize, int categoryID)
        {
            var category = db.Tags.FirstOrDefault(x => x.ID == categoryID);
            var tempResult = db.Recipes.Where(x => x.Approved && x.Categories.Any(y => y.Tag.ID == categoryID));
            RecipeByCategoryResultModel result = new RecipeByCategoryResultModel();
            result.MaxCount = tempResult.Count();
            var recipeResult = tempResult.OrderByDescending(x => x.DateCreated).Select(x => new
            {
                x.ID,
                x.ImageUrl,
                x.CroppedUrl,
                x.Title,
                x.Difficulty,
                LikeCount = x.UserLikes.Count(),
                CommentCount = x.Comments.Count()

            }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            result.Recipes = recipeResult.Select(x =>
                new Recipe
                {
                    ID = x.ID,
                    ImageUrl = x.ImageUrl,
                    CroppedUrl = x.CroppedUrl,
                    Title = x.Title,
                    Difficulty = x.Difficulty,
                    CommentCount = x.CommentCount,
                    LikeCount = x.LikeCount
                }).ToList();
            result.Category = category;
            return result;
        }


        public void Like(int recipeID, int userID)
        {
            var userLikeRecipe = db.UserRecipes.FirstOrDefault(x => x.Recipe.ID == recipeID && x.User.ID == userID);
            if (userLikeRecipe != null)
            {
                db.UserRecipes.Remove(userLikeRecipe);
                db.SaveChanges();
            }
            else
            {
                var recipe = db.Recipes.FirstOrDefault(x => x.ID == recipeID);
                var user = db.Users.FirstOrDefault(x => x.ID == userID);
                db.UserRecipes.Add(new UserRecipe { Recipe = recipe, User = user });
                db.SaveChanges();
            }
        }


        public List<User> GetRecipeLikes(int recipeID)
        {
            List<User> result = new List<User>();

            db.UserRecipes.Where(x => x.Recipe.ID == recipeID).Include(x => x.User).Select(x => new { FirstName = x.User.FirstName, LastName = x.User.LastName, ImageUrl = x.User.ImageUrl }).ToList().ForEach(x => result.Add(new User { FirstName = x.FirstName, LastName = x.LastName, ImageUrl = x.ImageUrl }));

            return result;
        }

        public bool UserLikesRecipe(int recipeID, int userID)
        {
            var result = db.UserRecipes.FirstOrDefault(x => x.Recipe.ID == recipeID && x.User.ID == userID);
            return result != null;
        }


        public bool CreateComment(int recipeID, int userID, string content)
        {
            var recipe = db.Recipes.FirstOrDefault(x => x.ID == recipeID);
            var user = db.Users.FirstOrDefault(x => x.ID == userID);

            if (recipe != null && user != null)
            {
                Comment cm = new Comment
                {
                    User = user,
                    Recipe = recipe,
                    Content = content,
                    DateCreated = DateTime.Now
                };
                db.Comments.Add(cm);
                int res = db.SaveChanges();
                return res > 0;
            }


            return false;
        }


        public List<Comment> GetCommentsForRecipe(int recipeID)
        {
            var tempres = db.Comments.Where(x => x.Recipe.ID == recipeID).Include(x => x.User).Select(x => new
                {
                    x.DateCreated,
                    x.Content,
                    x.User.ImageUrl,
                    x.User.FirstName,
                    x.User.LastName,
                    x.User.ID
                }).ToList();

            var result = tempres.Select(x => new Comment
            {
                Content = x.Content,
                DateCreated = x.DateCreated,
                User = new User
                {
                    ImageUrl = x.ImageUrl,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ID = x.ID
                }
            }).ToList();
            return result;
        }

        public RecipesResultModel SearchByIngredients(List<int> ingredientIDs, int page, int pageSize)
        {
            var tempResult = db.Recipes.Where(x => x.Approved && x.Ingredients.Any(y => ingredientIDs.Contains(y.Ingredient.ID))).Include(x => x.Ingredients).Include(x => x.Ingredients.Select(y => y.Ingredient)).OrderByDescending(x => x.Ingredients.Select(y => y.Ingredient.ID).Count(y => ingredientIDs.Contains(y))).Select(x => new
            {
                recipe = x,
                count = x.Ingredients.Select(y => y.Ingredient.ID).Count(y => ingredientIDs.Contains(y)),
                ingredients = x.Ingredients,
                LikeCount = x.UserLikes.Count(),
                CommentCount = x.Comments.Count()
            });



            RecipesResultModel result = new RecipesResultModel();
            result.MaxCount = tempResult.Count();
            result.Recipes = new List<Recipe>();
            var listResult = tempResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            foreach (var item in listResult)
            {
                Recipe tRecipe = item.recipe;
                tRecipe.MatchingIngredients = item.count;
                tRecipe.Ingredients = item.ingredients;
                tRecipe.CommentCount = item.CommentCount;
                tRecipe.LikeCount = item.LikeCount;
                result.Recipes.Add(tRecipe);

            }
            return result;
        }

        public RecipesResultModel GetRecipesForUser(int userID, int page, int pageSize)
        {
            var result = db.Users.Include(x => x.Recipes).Where(x => x.ID == userID).SelectMany(x => x.Recipes).Where(x => x.Approved).Select(x => new
            {
                x.ID,
                x.ImageUrl,
                x.CroppedUrl,
                x.Title,
                LikeCount = x.UserLikes.Count(),
                CommentCount = x.Comments.Count()
            }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            RecipesResultModel model = new RecipesResultModel();
            model.Recipes = result.Select(x => new Recipe { ID = x.ID, ImageUrl = x.ImageUrl, CroppedUrl = x.CroppedUrl, Title = x.Title, CommentCount = x.CommentCount, LikeCount = x.LikeCount }).ToList();

            return model;
        }


        public int CreateRecipe(RecipeModel model)
        {

            Recipe recipe = new Recipe();
            StringBuilder sb = new StringBuilder();
            int i = 1;
            var steps = model.Steps.Split('|');
            foreach (var item in steps)
            {
                sb.Append(string.Format("Чекор {0}.{1}", i, item));
                i++;
            }

            recipe.Content = sb.ToString();
            recipe.Title = model.Title;
            recipe.PreparationTime = model.PreparationTime.Value;
            recipe.NumberOfPeople = model.NumberOfPeople.Value;
            recipe.Description = model.Description;
            recipe.DateCreated = DateTime.Now;
            recipe.Rating = 0;
            recipe.Approved = model.isAdmin;
            recipe.Difficulty = model.Difficulty.Value;
            recipe.ImageUrl = model.ImageUrl;
            recipe.CroppedUrl = model.CroppedImageUrl;
            var ingredientIDs = model.Ingredients.Select(x => x.ID).ToList();
            var ingredients = db.Ingredients.Where(x => ingredientIDs.Contains(x.ID)).ToList();
            recipe.Ingredients = ingredients.Select(x => new IngredientsRecipe { Ingredient = x, Quantity = model.Ingredients.FirstOrDefault(y => y.ID == x.ID).Quantity }).ToList();

            var category = db.Tags.FirstOrDefault(x => x.ID == model.CategoryID);
            List<RecipeTag> categories = new List<RecipeTag>();
            categories.Add(new RecipeTag { Tag = category });
            recipe.Categories = categories;

            var user = db.Users.FirstOrDefault(x => x.ID == model.UserID);
            recipe.User = user;

            db.Recipes.Add(recipe);
            db.SaveChanges();

            return recipe.ID;

        }


        public Recipe GetRecipeForHome(int recipeID)
        {
            var recipe = db.Recipes.Where(x => x.ID == recipeID).Select(x => new { x.ImageUrl, x.Title, x.Description, x.ID }).FirstOrDefault();

            return new Recipe
            {
                ID = recipe.ID,
                Title = recipe.Title,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl
            };
        }


        public RecipesResultModel GetUnApproved(int page, int pageSize)
        {
            var tempResult = db.Recipes.Where(x => !x.Approved).Select(x => new
            {
                x.ID,
                x.Title,
                x.DateCreated,
                UserID = x.User.ID,
                Username = x.User.Username
            }).OrderBy(x => x.DateCreated);

            RecipesResultModel result = new RecipesResultModel
            {
                MaxCount = tempResult.Count()
            };

            var listResult = tempResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            result.Recipes = listResult.Select(x => new Recipe
            {
                ID = x.ID,
                Title = x.Title,
                DateCreated = x.DateCreated,
                User = new User { ID = x.UserID, Username = x.Username }
            }).ToList();

            return result;
        }


        public void ApproveRecipe(int recipeID, bool approved)
        {
            var recipe = db.Recipes.Find(recipeID);
            recipe.Approved = approved;
            db.SaveChanges();
        }
    }
}