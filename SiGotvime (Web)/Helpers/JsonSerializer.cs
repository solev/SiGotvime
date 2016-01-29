using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiGotvime.Web.Helpers
{
    public class JsonSerializer
    {
        public static System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer{MaxJsonLength = Int32.MaxValue};
    }
}