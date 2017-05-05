using SiGotvime.Data;
using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.Repository
{
    public class IngredientRepository : IIngredientRepository
    {
        private FoodDatabase db;

        public IngredientRepository(FoodDatabase _db)
        {
            db = _db;
        }

        public List<Models.Ingredient> GetAll()
        {
            var ingredients = db.Ingredients.ToList();
            return ingredients;
        }

        public Models.Ingredient GetById(int id)
        {
            var ingredient = db.Ingredients.FirstOrDefault(x => x.ID == id);
            return ingredient;

        }

        public bool Add(Models.Ingredient model)
        {
            db.Ingredients.Add(model);
            int res = db.SaveChanges();
            return res > 0;
        }

        public bool Edit(Models.Ingredient model)
        {
            db.Entry(model).State = EntityState.Modified;
            int res = db.SaveChanges();
            return res > 0;
        }


        public Models.Ingredient GetByName(string name)
        {
            var ingredient = db.Ingredients.FirstOrDefault(x => x.Name == name);
            return ingredient;
        }


        public bool Delete(int id)
        {
            var ing = db.Ingredients.FirstOrDefault(x => x.ID == id);
            if(ing != null)
            {
                db.Ingredients.Remove(ing);
            }
            return db.SaveChanges() > 0;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}