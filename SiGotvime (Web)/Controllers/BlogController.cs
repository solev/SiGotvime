using Ganss.XSS;
using SiGotvime.Data.Models;
using SiGotvime.Data.Repository;
using SiGotvime.Data.ViewModels;
using SiGotvime.Utilities;
using SiGotvime__Web_.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        // GET: Blog
        public ActionResult Index()
        {            
            var result = _blogRepository.AllPosts(1, 10);
            result.BlogPosts.ForEach(x =>
            {
                x.ImageUrl = CommonHelper.createImageUrl(x.ImageUrl);
                x.User.ImageUrl = CommonHelper.createImageUrl(x.User.ImageUrl);
            });              
          
            return View(result);
        }

        [Authorize]
        public ActionResult NewBlogPost()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateBlogPost(BlogPost post, PictureUploadModel model)
        {
            ImageHelper imageHelper = new ImageHelper { Width = 1500 };
            Image img = Image.FromStream(model.ImageToUpload.InputStream);
            var cropped = imageHelper.cropImage(img, new Rectangle { X = (int)model.imgx, Y = (int)model.imgy, Width = (int)model.imgw, Height = (int)model.imgh });
            var picTitle = imageHelper.SaveNewImage(cropped,Path.GetFileName(model.ImageToUpload.FileName));
            post.ImageUrl = String.Format("~/Content/images/{0}", picTitle.ImageName);
            post.DateCreated = DateTime.Now;
            post.Approved = false;
            post.User = new User { ID = Env.UserID() };
            _blogRepository.CreatePost(post);

            return RedirectToAction("BlogPostSaved");
        }

        public ActionResult GetPost(int id)
        {
            var sanitizer = new HtmlSanitizer();
            
            sanitizer.AllowedTags.Add("iframe");
            sanitizer.AllowedSchemes.Add("data");
            BlogPost blogpost = _blogRepository.GetPost(id);
            blogpost.ImageUrl = CommonHelper.createImageUrl(blogpost.ImageUrl);
            blogpost.User.ImageUrl = CommonHelper.createImageUrl(blogpost.User.ImageUrl);
           // blogpost.Content = sanitizer.Sanitize(blogpost.Content);

            blogpost.Comments.ToList().ForEach(x =>
            {
                x.User.ImageUrl = CommonHelper.createImageUrl(x.User.ImageUrl);
            });
            return View(blogpost);
        }

        public ActionResult BlogPostSaved()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(int BlogPostID, string content)
        {
            int userID = Env.UserID();
            _blogRepository.CreateComment(userID, BlogPostID, content);
            var comments = _blogRepository.GetCommentsForBlogpost(BlogPostID);
            comments.ForEach(x =>
            {
                x.User.ImageUrl = CommonHelper.createImageUrl(x.User.ImageUrl);
            });
            return PartialView("_CommentSection", comments);
        }
    }
}