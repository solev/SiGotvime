using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.Models
{
    public class Ingredient
    {
        public int ID { get; set; }        
        public string Name { get; set; }
        public ICollection<IngredientsRecipe> Recipes { get; set; }
    }
}