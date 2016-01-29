using SiGotvime.Data;
using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Food_App_Service.Controllers
{
    [Authorize]
    public class ReplicationController : Controller
    {
        // GET: Replication
        FoodDatabase db = new FoodDatabase();
        ReplicationDatabase replicationDb = new ReplicationDatabase();


        public string ReplicateDatabase()
        {
            var recipes = replicationDb.Recipes.ToList();
            var user = replicationDb.Users.FirstOrDefault(x=>x.ID == 1);
            foreach(var item in recipes)
            {
                item.User = user;
            }
            replicationDb.SaveChanges();
            
            return "success";
        }

        public string test()
        {
            Ingredient ing = new Ingredient { Name = "Test ing" };
            Recipe recipe = new Recipe { Title = "Test recipe", Content = "asdasd", DateCreated = DateTime.Now, Rating = 2 };



            replicationDb.IngredientsInRecipe.Add(new IngredientsRecipe { Ingredient = ing, Recipe = recipe, Quantity = "50" });
            replicationDb.SaveChanges();
            return "success";
        }
    }
}