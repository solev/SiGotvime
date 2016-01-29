using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.ViewModels
{
    public class RecipeListingModel
    {
        public int Count { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}