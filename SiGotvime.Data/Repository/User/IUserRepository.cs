using SiGotvime.Data.Models;
using SiGotvime.Data.Result_Models;
using SiGotvime.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public interface IUserRepository : IDisposable
    {
        User Authenticate(string username, string password);
        User GetUserById(int userID);
        User Insert(User user);
        void UpdateUser(User user);
        void UpdateUserProperty(int userID, string property, string value);
        FacebookUser GetFacebookUserById(string fbId);
        FacebookUser InsertFacebookUser(FacebookUser user);
        void UpdateFacebookUser(FacebookUser user);
        void CreateAccount(User user);
        bool emailExists(string email);
        ProfileViewModel GetProfileInfo(int userID);
        List<Recipe> GetUserRecipes(int userID);
        List<Recipe> GetUserFavouriteRecipes(int userID);
        User GetFeaturedUser(int UserID);
        List<BlogPost> GetUserBlogPosts(int userID);
        ListUsersResult GetAllUsers(int page, int PageSize);
    }
}
