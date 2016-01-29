using SiGotvime.Data.Models;
using SiGotvime.Data.Result_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public interface IBlogRepository
    {
        void CreatePost(BlogPost post);
        BlogPost GetPost(int postID);
        BlogPostResultModel AllPosts(int page, int pageSize);
        bool CreateComment(int userID, int BlogPostID, string content);
        List<Comment> GetCommentsForBlogpost(int blogPostID);
    }
}
