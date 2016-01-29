using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data
{
    public class ReplicationDatabase : DbContext
    {
        public ReplicationDatabase()
            : base("name = SiGotvimeDB") 
        {
            this.Configuration.LazyLoadingEnabled = false;            
        }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<IngredientsRecipe> IngredientsInRecipe { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UserRecipe> UserRecipes { get; set; }
    }
}
