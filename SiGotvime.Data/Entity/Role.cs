using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
