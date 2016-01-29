using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Result_Models
{
    public class BlogPostResultModel
    {
        public List<BlogPost> BlogPosts{ get; set; }
        public int MaxCount { get; set; }
    }
}
