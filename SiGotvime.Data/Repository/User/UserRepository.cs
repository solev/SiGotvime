using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SiGotvime.Data.ViewModels;

namespace SiGotvime.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private FoodDatabase db;

        public UserRepository(FoodDatabase _db)
        {
            db = _db;
        }
        public User Authenticate(string username, string password)
        {
            var result = db.Users.Include(x=>x.Roles).FirstOrDefault(x => x.Username == username && x.Password == password);            
            return result;
        }

        public void Dispose()
        {
            db.Dispose();
        }


        public User GetUserById(int userID)
        {
            var user = db.Users.FirstOrDefault(x => x.ID == userID);

            return user;
        }

        public User Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        public void UpdateUser(User user)
        {
            var result = db.Users.FirstOrDefault(x => x.ID == user.ID);
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.ImageUrl = user.ImageUrl;
            result.Username = user.Username;
            db.SaveChanges();
        }
        
        public void UpdateUserProperty(int userID, string property, string value)
        {
            var user = db.Users.FirstOrDefault(x => x.ID == userID);

            if(property == "FirstName")
                user.FirstName = value;
            else if(property == "LastName")
                user.LastName = value;
            else if(property == "DOB")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    DateTime dt;
                    if (DateTime.TryParse(value, out dt))
                    {
                        user.DateOfBirth = dt;
                    }
                }
                else
                {
                    user.DateOfBirth = null;
                }                
            }
            else if(property == "Email")
            {
                user.Email = value;
            }
            else if(property == "Telephone")
            {
                user.Telephone = value;
            }

            db.SaveChanges();
        }

        public FacebookUser GetFacebookUserById(string fbId)
        {
            var result = db.FacebookUsers.Include(x => x.User).FirstOrDefault(x => x.FacebookID == fbId);
            return result;
        }

        public FacebookUser InsertFacebookUser(FacebookUser user)
        {
            db.FacebookUsers.Add(user);
            db.SaveChanges();
            return user;
        }

        public void UpdateFacebookUser(FacebookUser user)
        {
            var fbUser = GetFacebookUserById(user.FacebookID);
            UpdateUser(fbUser.User);
            fbUser.Gender = user.Gender;
            fbUser.Link = user.Link;
            db.SaveChanges();
        }


        public void CreateAccount(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();            
        }

        public bool emailExists(string email)
        {
            var result = db.Users.FirstOrDefault(x => x.Username.ToLower() == email.ToLower());
            return result != null;
        }

        public ProfileViewModel GetProfileInfo(int userID)
        {            
            var user = db.Users.Where(x => x.ID == userID).Include(x => x.RecipeLikes).Select(x => new
            {
                x.ID,
                x.FirstName,
                x.LastName,
                x.ImageUrl,
                RecipeCount = x.Recipes.Count(),
                BlogPostCount = x.BlogPosts.Count(),
                Email = x.Email,
                Tel = x.Telephone,
                DOB = x.DateOfBirth
            }).FirstOrDefault();

            ProfileViewModel result = new ProfileViewModel();
            if(user!=null)
            {
                result = new ProfileViewModel
                {
                    UserID = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageUrl = user.ImageUrl,
                    RecipesSubmitted = user.RecipeCount,
                    PostsSubmitted = user.BlogPostCount,
                    Telephone = user.Tel,
                    Email = user.Email,
                    DateOfBirth = user.DOB
                };
            }
            return result;
        }

        public List<Recipe> GetUserRecipes(int userID)
        {
            var recipes = db.Recipes.Where(x => x.User.ID == userID).Include(x => x.UserLikes).Include(x => x.Comments).Select(x =>
                new
                {
                    ID = x.ID,
                    Title = x.Title,
                    CroppedImage = x.CroppedUrl, 
                    ImageUrl = x.ImageUrl,
                    Difficulty = x.Difficulty,
                    Likes = x.UserLikes.Count(),
                    CommentCount = x.Comments.Count(),
                    Approved = x.Approved
                }).ToList();

            List<Recipe> result = recipes.Select(x => new Recipe
            {
                ID = x.ID,
                Title = x.Title,
                CroppedUrl = x.CroppedImage??x.ImageUrl,
                Difficulty = x.Difficulty,
                LikeCount = x.Likes,
                CommentCount = x.CommentCount,
                Approved = x.Approved
            }).ToList();

            return result;
        }

        public List<Recipe> GetUserFavouriteRecipes(int userID)
        {          

            var recipes = db.UserRecipes.Where(x => x.User.ID == userID && x.Recipe.Approved).Include(x => x.Recipe).Include(x=>x.Recipe.Comments).Include(x=>x.Recipe.UserLikes).Select(x =>
                new
                {
                    ID = x.ID,
                    Title = x.Recipe.Title,
                    CroppedImage = x.Recipe.CroppedUrl,
                    ImageUrl = x.Recipe.ImageUrl,
                    Difficulty = x.Recipe.Difficulty,
                    Likes = x.Recipe.UserLikes.Count(),
                    CommentCount = x.Recipe.Comments.Count()                    
                }).ToList();

            List<Recipe> result = recipes.Select(x => new Recipe
            {
                ID = x.ID,
                Title = x.Title,
                CroppedUrl = x.CroppedImage ?? x.ImageUrl,
                Difficulty = x.Difficulty,
                LikeCount = x.Likes,
                CommentCount = x.CommentCount                
            }).ToList();

            return result;
        }

        public User GetFeaturedUser(int UserID)
        {
            var result = db.Users.Where(x => x.ID == UserID).Select(x => new { x.FirstName, x.LastName, x.ImageUrl,x.ID }).FirstOrDefault();

            return new User
            {
                ID = result.ID,
                ImageUrl = result.ImageUrl,
                FirstName = result.FirstName,
                LastName = result.LastName
            };
        }


        public List<BlogPost> GetUserBlogPosts(int userID)
        {
            var tempResult = db.BlogPosts.Where(x => x.User.ID == userID && x.Approved).Select(x => new
            {
                x.BlogPostID,
                x.ImageUrl,
                x.Title,
                x.DateCreated,
                CommentCount = x.Comments.Count(),
                x.Description
            }).ToList();

            List<BlogPost> result = tempResult.Select(x => new BlogPost
            {
                BlogPostID = x.BlogPostID,
                ImageUrl = x.ImageUrl,
                Title = x.Title,
                DateCreated = x.DateCreated,
                CommentCount = x.CommentCount,
                Description = x.Description
            }).ToList();

            return result;
        }
    }
}
