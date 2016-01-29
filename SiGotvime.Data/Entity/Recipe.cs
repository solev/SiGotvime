using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.Models
{

    public enum RecipeDifficulty
    {
        [Display(Name = "Лесна")]
        Easy = 1 ,
        [Display(Name = "Средна")]
        Medium,
        [Display(Name = "Тешка")]
        Hard
    }

    public class Recipe
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int PreparationTime { get; set; }
        public string CroppedUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public int NumberOfPeople { get; set; }
        public int Rating { get; set; }
        public bool Approved { get; set; }
        public RecipeDifficulty Difficulty { get; set; }
        public string Description { get; set; }
        public virtual ICollection<IngredientsRecipe> Ingredients { get; set; }
        public virtual ICollection<RecipeTag> Categories{ get; set; }
        public virtual ICollection<UserRecipe> UserLikes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual User User{ get; set; }

        [NotMapped]
        public int MatchingIngredients { get; set; }
        [NotMapped]
        public int LikeCount { get; set; }
        [NotMapped]
        public int CommentCount { get; set; }


    }
}