using SiGotvime.Data.Repository;
using SiGotvime.Data.Models;
using SiGotvime.Data.ViewModels;
using SiGotvime.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;


namespace Food_App_Service.Controllers
{    
    [RoutePrefix("api/Ingredient")]  
    public class IngredientsController : ApiController
    {
        IIngredientRepository ingredientRepository;
        
        public IngredientsController(IngredientRepository _ingredientRepository)
        {
            ingredientRepository = _ingredientRepository;
        }

        [Route("getAll")]
        [HttpGet]
        public IEnumerable<Ingredient> GetIngredients()
        {            
            return ingredientRepository.GetAll().OrderBy(x=>x.Name);
        }

        [HttpGet]
        [Route("getStatuses")]

        public IEnumerable<Status> GetSelectedStatuses()
        {
            FoodDatabase db = new FoodDatabase();
            return db.Statuses.Where(x => x.Selected);
        }

        [HttpGet]
        [Route("videoUrl")]
        public string videourl()
        {
            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/video"));
            var filename = Path.GetFileName(files[0]);

            return filename;
        }
        
    }
    
   
}
