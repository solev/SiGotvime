using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SiGotvime.Data
{
    public class FoodDatabase : DbContext
    {
        public FoodDatabase(): base("name = SiGotvimeDB") 
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
        public DbSet<FacebookUser> FacebookUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Achievment> Achievments { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Settings> Settings{ get; set; }
        public DbSet<Message> Messages{ get; set; }
        public DbSet<Plan> Plans { get; set; }
    }
}