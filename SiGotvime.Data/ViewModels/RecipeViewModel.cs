using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.ViewModels
{
    public class RecipeViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string CroppedUrl { get; set; }
        public int NumberOfPeople { get; set; }
        public RecipeDifficulty Difficulty { get; set; }
        public int PreparationTime { get; set; }
        public int Rating { get; set; }
        public bool Approved { get; set; }
        public HttpPostedFileBase  ImageToUpload { get; set; }

        public IEnumerable<IngredientViewModel> ingredients { get; set; }
        public IEnumerable<int> tags { get; set; }

    }
}