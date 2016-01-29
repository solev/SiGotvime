using SiGotvime.Data.Models;
using SiGotvime.Data.Result_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.ViewModels
{
    public class SearchViewModel
    {
        public List<Ingredient> Ingredients { get; set; }
        public List<Tag> Categories { get; set; }
        public List<Ingredient> SelectedIngredients { get; set; }
        public RecipesResultModel result { get; set; }
    }
}
