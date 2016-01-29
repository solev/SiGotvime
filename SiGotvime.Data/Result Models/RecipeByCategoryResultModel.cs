using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Result_Models
{
    public class RecipeByCategoryResultModel : RecipesResultModel
    {
        public Tag Category { get; set; }
    }
}
