using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.ViewModels
{
    public class PagerViewModel
    {
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int maxCount { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
        public int id { get; set; }

    }
}
