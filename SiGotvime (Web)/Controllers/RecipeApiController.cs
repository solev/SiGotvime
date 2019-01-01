using SiGotvime.Data.Models;
using SiGotvime.Data.Repository;
using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace SiGotvime__Web_.Controllers
{
    [RoutePrefix("api/Recipes")]
    public class RecipeApiController : ApiController
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeApiController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [Route("All")]
        public List<Recipe> GetAll([FromUri]int page = 1)
        {            
            var latestRecipes = _recipeRepository.GetLatestRecipes(page, Constants.PageSize);
            return latestRecipes.Recipes;
        }        

        [Route("Get/{id}")]
        public IHttpActionResult Get(int id)
        {
            var recipe = _recipeRepository.GetFullRecipeById(id);
            if (recipe == null)
                return NotFound();

            return Ok(recipe);
        }

    }
}
