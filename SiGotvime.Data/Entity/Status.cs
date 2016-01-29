using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Data.Models
{
    public class Status
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string SubContent { get; set; }
        public bool Selected { get; set; }
    }
}