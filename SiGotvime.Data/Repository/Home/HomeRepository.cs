using SiGotvime.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public class HomeRepository : IHomeRepository
    {

        private FoodDatabase db;        
        public HomeRepository(FoodDatabase _db)
        {
            db = _db;            
        }
        public HomePageViewModel GetHomePageData()
        {
            HomePageViewModel result = new HomePageViewModel();

            result.Members = db.Users.Count();
            result.Recipes = db.Recipes.Count();
            result.Comments = db.Comments.Count();
            result.Articles = db.BlogPosts.Count();
            return result;

        }
    }
}
