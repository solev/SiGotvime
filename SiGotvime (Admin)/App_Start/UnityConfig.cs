using System.Web.Mvc;
using SiGotvime.Data.Repository;
using SiGotvime.Data;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
namespace Food_App_Service
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IRecipeRepository, RecipeRepository>();
            container.RegisterType<IIngredientRepository, IngredientRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<FoodDatabase>(new PerResolveLifetimeManager());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}