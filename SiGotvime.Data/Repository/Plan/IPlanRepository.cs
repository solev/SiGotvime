using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public interface IPlanRepository
    {
        bool CreatePlan(DateTime date, int UserID, int RecipeID);
        List<Plan> GetPlansForUser(DateTime startDate, DateTime endDate, int UserID); 
    }
}
