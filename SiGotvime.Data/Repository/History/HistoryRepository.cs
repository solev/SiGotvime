using SiGotvime.Data.Models;
using SiGotvime.Data.Result_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository.History
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
            var recipeIds = db.History.Where(x => x.Type == Entity.HistoryType.Recipe).OrderByDescending(x => x.DateCreated).Skip((page-1)*pageSize).Take(pageSize).Select(x => x.EntityID);

            var tempRes = db.Recipes.Where(x => recipeIds.Contains(x.ID)).Select(x => new
            {
                x.ID,
                x.ImageUrl,
                x.CroppedUrl,
                x.Title,
                x.Difficulty,
                LikeCount = x.UserLikes.Count(),
                CommentCount = x.Comments.Count()
            });

            result.MaxCount = tempRes.Count();
            result.Recipes = tempRes.ToList().Select(x =>
                new Recipe
                {
                    ID = x.ID,
                    ImageUrl = x.ImageUrl,
                    CroppedUrl = x.CroppedUrl,
                    Title = x.Title,
                    CommentCount = x.CommentCount,
                    LikeCount = x.LikeCount
                }).ToList();

            return result;
        }
    }
}
