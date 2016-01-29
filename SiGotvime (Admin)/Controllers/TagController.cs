using SiGotvime.Data.Repository;
using SiGotvime.Data.Models;
using SiGotvime.Data.ViewModels;
using SiGotvime.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Food_App_Service.Controllers
{
    [RoutePrefix("api/Tags")]
    public class TagController : ApiController
    {
        
        FoodDatabase db = new FoodDatabase();

        [Route("getAll")]
        [HttpGet]
        public List<Tag> GetAll()
        {
            return db.Tags.ToList();
        }
    }
}
