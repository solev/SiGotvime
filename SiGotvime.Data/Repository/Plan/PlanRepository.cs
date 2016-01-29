using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public class PlanRepository : IPlanRepository
    {
        FoodDatabase db;
        public PlanRepository(FoodDatabase _db)
        {
            db = _db;
        }

        public bool CreatePlan(DateTime date, int UserID, int RecipeID)
        {
            var user = db.Users.Find(UserID);
            var recipe = db.Recipes.Find(RecipeID);

            Plan plan = new Plan
            {
                DateCreated = date,
                User = user,
                Recipe = recipe                
            };

            db.Plans.Add(plan);
            int res = db.SaveChanges();
            return res > 0;
        }


        public List<Plan> GetPlansForUser(DateTime startDate, DateTime endDate, int UserID)
        {
            var result = db.Plans.Include(x=>x.Recipe).Where(x => x.User.ID == UserID && DbFunctions.TruncateTime(x.DateCreated) >= startDate.Date && DbFunctions.TruncateTime(x.DateCreated) < endDate.Date).ToList();
            return result;
        }
    }
}
