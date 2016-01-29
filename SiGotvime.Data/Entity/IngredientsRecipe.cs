using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.Models
{
    public class IngredientsRecipe
    {
        public int ID { get; set; }

        public virtual Ingredient Ingredient { get; set; }
        public virtual Recipe Recipe { get; set; }
        public string Quantity { get; set; }

    }
}