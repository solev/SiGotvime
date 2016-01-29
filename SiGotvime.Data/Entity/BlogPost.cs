using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiGotvime.Data.Models
{
    public class BlogPost
    {
        [Key]
        public int BlogPostID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }        
        public DateTime DateCreated { get; set; }
        public bool Approved { get; set; }

        public virtual User User{ get; set; }
        public virtual ICollection<Comment> Comments { get; set; }        

        
        [NotMapped]
        public int CommentCount { get; set; }
    }
}
