
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Models
{
    public class UserRecipe
    {
        public int ID { get; set; }
        public virtual User User { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
