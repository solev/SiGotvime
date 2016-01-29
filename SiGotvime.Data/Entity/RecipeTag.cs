using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.Models
{
    public class RecipeTag
    {
        public int ID { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual Tag Tag { get; set; }
    }
}