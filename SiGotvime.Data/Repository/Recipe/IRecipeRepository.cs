﻿using SiGotvime.Data.DTO;
using SiGotvime.Data.Models;
using SiGotvime.Data.Result_Models;
using SiGotvime.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public interface IRecipeRepository :IDisposable
    {
        Recipe FindById(int id);
        bool Add(RecipeViewModel model, int userID = 0);
        RecipeListingModel GetAll(int page, string search = null);
        RecipeDto GetFullRecipeById(int id);
        Recipe GetCompleteRecipe(int recipeID,int AdminUserID=0);

        //OLD
        IEnumerable<RecipeViewModel> GetByCategory(int categoryID,int page);
        //OLD
        IEnumerable<RecipeViewModel> GetEasiest(int page);

        RecipeListingModel SearchRecipes(string search, int page = 1);
        bool Edit(RecipeViewModel model, int id);
        bool Delete(int id);
        //OLD
        List<int> GetLatest(int page);
        
        RecipesResultModel GetLatestRecipes(int page,int pageSize);
        RecipeByCategoryResultModel GetRecipesByCategory(int page, int pageSize, int categoryID);
        void Like(int recipeID, int userID);
        List<User> GetRecipeLikes(int recipeID);

        bool UserLikesRecipe(int recipeID, int userID);
        bool CreateComment(int recipeID, int userID, string content);
        List<Comment> GetCommentsForRecipe(int recipeID);
        RecipesResultModel SearchByIngredients(List<int> ingredientIDs,int page,int pageSize);

        RecipesResultModel GetRecipesForUser(int userID, int page, int pageSize);

        int CreateRecipe(RecipeModel recipe);
        Recipe GetRecipeForHome(int recipeID);
        RecipesResultModel GetUnApproved(int page, int pageSize);
        void ApproveRecipe(int recipeID,bool approved);
    }
}
