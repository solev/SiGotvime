using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.ViewModels
{
    public class ProfileViewModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public int PostsSubmitted { get; set; }
        public int RecipesSubmitted { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
