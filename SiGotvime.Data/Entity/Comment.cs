using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public virtual User User{ get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual BlogPost BlogPost { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
