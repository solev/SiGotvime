using SiGotvime.Data;
using SiGotvime.Utilities;
using SiGotvime__Web_.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    [Authorize]
    public class HelperController : Controller
    {
        private FoodDatabase db;

        public HelperController(FoodDatabase _db)
        {
            db = _db;
        }
        
        //public ActionResult AllImages()
        //{
        //    string path = "~/Content/images/";
        //    List<string> files = Directory.GetFiles(Server.MapPath(path)).ToList();
        //    var newFiles = files.Select(x => string.Concat(path, Path.GetFileName(x)));
        //    return View(newFiles.ToList());
        //}

        public ActionResult DateHelper()
        {
            return View();
        }

        public string FixImages()
        {
            string path = "~/Content/images/";
            List<string> files = Directory.GetFiles(Server.MapPath(path)).ToList();
            ImageHelper imageHelper = new ImageHelper { Width = 1500 };
            StringBuilder sb = new StringBuilder();
            foreach(var file in files)
            {
                FileInfo f = new FileInfo(file);
                
                if(f.Length > 350000)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(file);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    Image img = Image.FromStream(ms);
                    ms.Flush();
                    var imgResult = imageHelper.ScaleAndSave(img, f.Name);

                }
                else if(f.Length < 100000)
                {
                    sb.Append(string.Format("{0}\n", f.Name));
                }
                
            }
            return "success";
        }


    }
}