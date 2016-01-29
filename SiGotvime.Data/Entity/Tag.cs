using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.Models
{
    public class Tag
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RecipeTag> Recipes { get; set; }

    }
}