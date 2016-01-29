using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SiGotvime.Data.Result_Models;

namespace SiGotvime.Data.Repository
{
    public class BlogRepository : IBlogRepository
    {

        private FoodDatabase db;

        public BlogRepository(FoodDatabase _db)
        {
            db = _db;
        }

        public void CreatePost(BlogPost post)
        {
            post.User = db.Users.FirstOrDefault(x => x.ID == post.User.ID);
            db.BlogPosts.Add(post);
            db.SaveChanges();
        }

        public BlogPost GetPost(int postID)
        {
            var post = db.BlogPosts.Where(x => x.BlogPostID == postID && x.Approved).Include(x => x.User).Include(x=>x.Comments.Select(y=>y.User)).Select(x =>
                new
                {
                    x.BlogPostID,
                    x.ImageUrl,
                    x.Title,
                    x.Content,
                    x.DateCreated,
                    x.Approved,
                    UserImage = x.User.ImageUrl,
                    x.User.FirstName,
                    x.User.LastName,
                    x.User.ID,
                    Comments = x.Comments.Select(y=>new{y.ID,y.DateCreated,y.Content,userid = y.User.ID,y.User.ImageUrl,y.User.FirstName,y.User.LastName})
                }).FirstOrDefault();

            if (post == null)
                return null;

            BlogPost result = new BlogPost
            {
                BlogPostID = post.BlogPostID,
                Title = post.Title,
                ImageUrl = post.ImageUrl,
                Content = post.Content,
                DateCreated = post.DateCreated,
                Approved = post.Approved,
                User = new User
                {
                    ID = post.ID,
                    ImageUrl = post.UserImage,
                    FirstName = post.FirstName,
                    LastName = post.LastName
                },
                Comments = post.Comments.Select(x => new Comment 
                {
                    ID = x.ID,
                    DateCreated = x.DateCreated,
                    Content = x.Content,
                    User = new User
                    {
                        ID = x.userid,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        ImageUrl = x.ImageUrl
                    }
                }).ToList()
            };
            return result;
        }


        public BlogPostResultModel AllPosts(int page, int pageSize)
        {
            BlogPostResultModel result = new BlogPostResultModel();

            var tempRes = db.BlogPosts.Include(x => x.User).Include(x=>x.Comments).Where(x => x.Approved);
            result.MaxCount = tempRes.Count();
            var posts = tempRes.OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new
            {
                x.BlogPostID,
                x.Title,
                x.ImageUrl,
                x.Description,
                x.Content,
                x.DateCreated,
                x.Approved,
                x.User.ID,
                x.User.FirstName,
                x.User.LastName,
                UserImageUrl = x.User.ImageUrl,
                CommentCount = x.Comments.Count()
            }).ToList();


            List<BlogPost> list = posts.Select(x => 
                new BlogPost
                { 
                    Title = x.Title,
                    Description = x.Description ,
                    BlogPostID = x.BlogPostID,
                    ImageUrl = x.ImageUrl, 
                    Content = x.Content, 
                    DateCreated = x.DateCreated, 
                    Approved = x.Approved, 
                    CommentCount = x.CommentCount,
                    User = new User
                    {
                        ID = x.ID, 
                        FirstName = x.FirstName,
                        LastName = x.LastName, 
                        ImageUrl = x.UserImageUrl
                    }
                }).ToList();

            result.BlogPosts = list;
            return result;
        }


        public bool CreateComment(int userID, int BlogPostID, string content)
        {
            var user = db.Users.Find(userID);
            var blogpost = db.BlogPosts.Find(BlogPostID);

            if(user!=null && blogpost!=null)
            {
                Comment comment = new Comment
                {
                    User = user,
                    BlogPost = blogpost,
                    DateCreated = DateTime.Now,
                    Content = content
                };
                db.Comments.Add(comment);
                int res = db.SaveChanges();
                return res > 0;
            }

            return false;
        }


        public List<Comment> GetCommentsForBlogpost(int blogPostID)
        {
            var tempres = db.Comments.Where(x => x.BlogPost.BlogPostID == blogPostID).Include(x => x.User).Select(x => new
            {
                x.DateCreated,
                x.Content,
                x.User.ImageUrl,
                x.User.FirstName,
                x.User.LastName,
                x.User.ID
            }).ToList();

            var result = tempres.Select(x => new Comment
            {
                Content = x.Content,
                DateCreated = x.DateCreated,
                User = new User
                {
                    ImageUrl = x.ImageUrl,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ID = x.ID
                }
            }).ToList();
            return result;
        }
    }
}
