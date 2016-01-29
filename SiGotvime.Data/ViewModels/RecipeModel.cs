using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiGotvime.Data.ViewModels
{
    public class RecipeModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public int? PreparationTime { get; set; }
        public int? NumberOfPeople { get; set; }
        public RecipeDifficulty? Difficulty { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }        
        public string ImageUrl { get; set; }
        public string Steps { get; set; }
        public HttpPostedFileBase ImageToUpload{ get; set; }

        public List<IngredientViewModel> Ingredients { get; set; }
        public List<Tag> Categories { get; set; }
    }
}
