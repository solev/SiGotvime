using SiGotvime.Data.Models;
using SiGotvime.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.DTO
{

    public class RecipeDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CroppedUrl { get; set; }
        public int NumberOfPeople { get; set; }
        public RecipeDifficulty Difficulty { get; set; }
        public int PreparationTime { get; set; }
        public int Rating { get; set; }
        public IEnumerable<IngredientViewModel> Ingredients { get; set; }
    }
}
