using SiGotvime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.ViewModels
{
    public class SearchModel
    {
        public List<int> ingredients { get; set; }
        public int taken { get; set; }
    }
}