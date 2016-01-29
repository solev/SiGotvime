using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public interface IIngredientRepository : IDisposable
    {
        List<Ingredient> GetAll();
        Ingredient GetById(int id);
        Ingredient GetByName(string name);

        bool Add(Ingredient model);
        bool Edit(Ingredient model);
        bool Delete(int id);
    }
}
