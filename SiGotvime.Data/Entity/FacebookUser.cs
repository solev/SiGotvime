using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Models
{
    public class FacebookUser
    {
        [Key]
        public string FacebookID{ get; set; }
        public string Link { get; set; }
        public string Gender { get; set; }
        public virtual User User { get; set; }
    }
}
