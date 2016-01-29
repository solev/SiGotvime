using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Models
{
    public class Message
    {
        public int ID { get; set; }
               
        public virtual User UserBy{ get; set; }               
        public virtual User UserTo { get; set; }

        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        
        [NotMapped]
        public string Email { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        [NotMapped]
        public string Validate { get; set; }
        
    }
}
