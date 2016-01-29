using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.ViewModels
{
    public class HomePageViewModel
    {
        public int Members { get; set; }
        public int Recipes { get; set; }
        public int Photos { get; set; }
        public int Comments { get; set; }
        public int Articles { get; set; }

        public Recipe RecipeOfTheDay { get; set; }
        public User FeaturedMember { get; set; }
        public List<Recipe>  LatestRecipes { get; set; }
        public List<BlogPost> BlogPosts { get; set; }
    }
}
