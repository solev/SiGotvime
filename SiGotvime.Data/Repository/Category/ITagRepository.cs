using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public interface ITagRepository
    {
        List<Tag> GetAll();
    }
}
