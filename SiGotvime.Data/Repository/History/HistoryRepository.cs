using SiGotvime.Data.Entity;
using SiGotvime.Data.Models;
using SiGotvime.Data.Result_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public class HistoryRepository : IHistoryRepository
    {
        private FoodDatabase db;

        public HistoryRepository(FoodDatabase _db)
        {
            db = _db;
        }

        public RecipesResultModel GetPreviousRecipesOfTheDay(int page, int pageSize)
        {
            RecipesResultModel result = new RecipesResultModel();
            var tempRes = db.History.Where(x => x.Type == Entity.HistoryType.Recipe).OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize)
                .Join(db.Recipes, a => a.EntityID, b => b.ID, (a, b) => new {x=b})
                .Select(x => new
                {
                    x.x.ID,
                    x.x.ImageUrl,
                    x.x.CroppedUrl,
                    x.x.Title,
                    x.x.Difficulty,
                    LikeCount = x.x.UserLikes.Count(),
                    CommentCount = x.x.Comments.Count()
                });

            //var tempRes = db.Recipes.Where(x => recipeIds.Contains(x.ID)).Select(x => new
            //{
            //    x.ID,
            //    x.ImageUrl,
            //    x.CroppedUrl,
            //    x.Title,
            //    x.Difficulty,
            //    LikeCount = x.UserLikes.Count(),
            //    CommentCount = x.Comments.Count()
            //});

            result.MaxCount = tempRes.Count();
            result.Recipes = tempRes.ToList().Select(x =>
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

            return result;
        }
        
        public bool InsertRecipeOfTheDay(int recipeID)
        {
            History recipeOfTheDay = new History
            {
                EntityID = recipeID,
                DateCreated = DateTime.Now,
                Type = HistoryType.Recipe
            };
            db.History.Add(recipeOfTheDay);
            int res = db.SaveChanges();
            return res > 0;
        }
    }
}
