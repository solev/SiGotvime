using SiGotvime.Data.Models;
using SiGotvime.Data.Repository;
using SiGotvime__Web_.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    [AdminAuthorize]
    public class IngredientController : Controller
    {        
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientController(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        // GET: Ingredient
        public ActionResult Index()
        {
            var ingretients = _ingredientRepository.GetAll();
            return View(ingretients);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Ingredient model)
        {
            if (model.Name == null || model.Name.Trim() == "")
            {
                ViewBag.error = "Името не смее да е празно!";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var temp = _ingredientRepository.GetByName(model.Name);
                if (temp != null)
                {
                    ViewBag.error = "Таа состојка веќе постои!";
                    return View(model);
                }
                bool res = _ingredientRepository.Add(model);
                if (res)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Ingredient ing = _ingredientRepository.GetById(id);

            if (ing == null)
                return HttpNotFound();

            return View(ing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ingredient model)
        {
            if (model.Name == null || model.Name.Trim() == "")
            {
                ViewBag.error = "Името не смее да е празно!";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                _ingredientRepository.Edit(model);
            }

            return RedirectToAction("Index");
        }

    }
}