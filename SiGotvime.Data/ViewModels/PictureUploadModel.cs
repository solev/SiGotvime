using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiGotvime.Data.ViewModels
{
    public class PictureUploadModel
    {
        public HttpPostedFileBase ImageToUpload { get; set; }
        public float imgx { get; set; }
        public float imgy { get; set; }
        public float imgw { get; set; }
        public float imgh { get; set; }
    }
}
