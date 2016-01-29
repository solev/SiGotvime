using SiGotvime.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private FoodDatabase db;
        public StatisticsRepository(FoodDatabase _db)
        {
            db = _db;            
        }
        public StatisticsViewModel AverageRecipeTime()
        {

            StatisticsViewModel result = new StatisticsViewModel();
            using(var transaction =  db.Database.BeginTransaction())
            {
                int count = db.Recipes.Count();
                decimal TimeSum = db.Recipes.Sum(x => x.PreparationTime);
                int minTime = db.Recipes.Min(x => x.PreparationTime);
                int maxTime = db.Recipes.Max(x => x.PreparationTime);
                transaction.Commit();

                result.AverageRecipeTime = TimeSum / count;
                result.MinimumRecipeTime = minTime;
                result.MaximumRecipeTime = maxTime;

                return result;
            }
        }
    }
}
