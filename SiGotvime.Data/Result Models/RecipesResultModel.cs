using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Result_Models
{
    public class RecipesResultModel
    {
        public int EntityID { get; set; }
        public List<Recipe> Recipes { get; set; }
        public int MaxCount { get; set; }
    }
}
