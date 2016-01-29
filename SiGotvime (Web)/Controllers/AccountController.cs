using SiGotvime.Data.Models;
using SiGotvime.Data.Repository;
using SiGotvime.Data.ViewModels;
using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SiGotvime__Web_.Controllers
{
    public class AccountController : Controller
    {
        
        private IUserRepository _userRepository;
        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            return View();
        }        
        
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            var emailExists = _userRepository.emailExists(model.Email);
            if(emailExists)
            {
                ViewBag.Message = "Email-от кој го избравте веќе постои!";
                return View(model);
            }

            var user = new User
            {
                Username = model.Email,
                Password = model.Password,
                ConfirmPassword = model.RepeatPassword,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageUrl = Constants.DefaultImgUrl
            };
            
            _userRepository.CreateAccount(user);
            user.ImageUrl = CommonHelper.createImageUrl(user.ImageUrl);
            CommonHelper.CreateCookie(user);
            return RedirectToAction("Profile", "User");
        }

        public ActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var user = _userRepository.Authenticate(model.Username, model.Password);
            if(user!=null)
            {
                user.ImageUrl = CommonHelper.createImageUrl(user.ImageUrl);
                CommonHelper.CreateCookie(user);
                                
                if(!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        public ActionResult Authentication()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult FBLogin(FBLoginModel model)
        {
            var checkUser = _userRepository.GetFacebookUserById(model.id);
            if(!User.Identity.IsAuthenticated)
            {                
                if(checkUser == null)
                {
                    var newUser = new User
                    {
                        FirstName = model.first_name,
                        LastName = model.last_name,
                        ImageUrl = model.url,
                        Password = "********",
                        ConfirmPassword = "********",
                        Username = model.id
                    };

                    _userRepository.Insert(newUser);
                    checkUser = new FacebookUser
                    {
                        FacebookID = model.id,
                        Gender = model.gender,
                        Link = model.link,
                        User = newUser
                    };
                    _userRepository.InsertFacebookUser(checkUser);
                }
                CommonHelper.CreateCookieFacebook(checkUser);
            }

            CheckFbDifferences(checkUser, model);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        
        private bool CheckFbDifferences(FacebookUser fbUser, FBLoginModel model)
        {
            bool ok = true;
            if(fbUser.Gender != model.gender)
            {
                ok = false;
                fbUser.Gender = model.gender;
            }

            if(fbUser.Link != model.link)
            {
                ok = false;
                fbUser.Link = model.link;
            }

            if(fbUser.User.FirstName != model.first_name)
            {
                ok = false;
                fbUser.User.FirstName = model.first_name;
            }

            if(fbUser.User.LastName != model.last_name)
            {
                fbUser.User.LastName = model.last_name;
                ok = false;
            }
            
            if(fbUser.User.ImageUrl != model.url)
            {
                fbUser.User.ImageUrl = model.url;
                ok = false;
            }

            if(!ok)
            {
                _userRepository.UpdateFacebookUser(fbUser);
                CommonHelper.CreateCookieFacebook(fbUser);
            }

            return ok;
        }
    }
}