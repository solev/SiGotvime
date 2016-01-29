using SiGotvime.Data.Repository;
using SiGotvime.Data.Models;
using SiGotvime.Data.ViewModels;
using SiGotvime.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SiGotvime.Utilities;

namespace Food_App_Service.Controllers
{
    public class UserController : Controller
    {
        FoodDatabase db = new FoodDatabase();
        private readonly IUserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var userToLogin = _userRepository.Authenticate(user.Username, user.Password);
            if(userToLogin != null && userToLogin.Roles.Any(x=>x.ID==1))
            {
                CommonHelper.CreateCookie(userToLogin);
                return RedirectToAction("Ingredients", "Admin");
            }

            ViewBag.error = "Корисничкото име или лозинката ви е погрешна!";
            return View();
        }

        [Authorize]
        public ActionResult Register()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Register(User model)
        {
            if(ModelState.IsValid)
            {
                var checkUser = db.Users.FirstOrDefault(x => x.Username == model.Username);
                if(checkUser!=null)
                {
                    ViewBag.error = "Корисничкото име е веќе зафатено!";
                    return View(model);
                }
                db.Users.Add(model);
                db.SaveChanges();
                ViewBag.success = "Корисникот е успешно додаден!";
                return View();
            }
            return View(model);
        }




        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        
	}
}