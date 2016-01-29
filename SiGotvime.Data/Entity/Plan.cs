using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Models
{
    public class Plan
    {
        [Key]
        public int PlanID { get; set; }
        public virtual Recipe Recipe { get; set; }        
        public virtual User User { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
