using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }        

        public virtual ICollection<Recipe> Recipes{ get; set; }
        public virtual ICollection<UserRecipe> RecipeLikes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Role> Roles{ get; set; }
        public virtual ICollection<Achievment> Achievments{ get; set; }
        public virtual ICollection<BlogPost> BlogPosts{ get; set; }
    }
}