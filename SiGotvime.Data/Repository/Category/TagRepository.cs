using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public class TagRepository : ITagRepository
    {
        private FoodDatabase db;

        public TagRepository(FoodDatabase _db)
        {
            db = _db;
        }



        public List<Tag> GetAll()
        {
            var result = db.Tags.ToList();
            return result;
        }
    }
}
