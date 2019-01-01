using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using System.Web.Http;
using SiGotvime.Data.Repository;
using SiGotvime.Data;

namespace SiGotvime__Web_
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
            container.RegisterType<IStatisticsRepository, StatisticsRepository>();
            container.RegisterType<IHomeRepository, HomeRepository>();
            container.RegisterType<ISettingsRepository, SettingsRepository>();
            container.RegisterType<IMessageRepository, MessageRepository>();
            container.RegisterType<IBlogRepository, BlogRepository>();
            container.RegisterType<IPlanRepository, PlanRepository>();
            container.RegisterType<IHistoryRepository, HistoryRepository>();
            container.RegisterType<FoodDatabase>(new PerResolveLifetimeManager());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}