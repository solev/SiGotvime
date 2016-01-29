using SiGotvime.Data.Repository;
using SiGotvime.Data.ViewModels;
using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanRepository _planRepository;

        public PlanController(PlanRepository planRepository)
        {
            _planRepository = planRepository;
        }
        // GET: Plan
        public ActionResult Index()
        {
                        
            return View();
        }

        public JsonResult GetPlans(DateTime start,DateTime end)
        {
            List<PlanViewModel> plans = new List<PlanViewModel>();
            if(User.Identity.IsAuthenticated)
            {
                int UserID = Env.UserID();
                var planResult = _planRepository.GetPlansForUser(start, end, Env.UserID());
                planResult.ForEach(x =>
                {
                    plans.Add(new PlanViewModel
                    {
                        start = x.DateCreated.ToShortDateString(),
                        title = x.Recipe.Title,
                        url = Url.Action("GetRecipe", "Recipe", new { id = x.Recipe.ID})
                    });
                });
            }
            
            
            return Json(plans,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreatePlan(DateTime value,int pk)
        {
            int UserID = Env.UserID();
            bool res = _planRepository.CreatePlan(value, UserID, pk);
            string message = res?"Успешно внесен рецепт во вашиот календар за планирање":"Внесувањето беше неуспешно,обидете се повторно";
            return Json(new JsonResponse { Success = true, Message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}