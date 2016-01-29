using SiGotvime.Data.Repository;
using SiGotvime.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiGotvime__Web_.Controllers
{
    [Authorize]
    public class StatisticController : Controller
    {
        private readonly IStatisticsRepository _statisticsRepository;
        // GET: Statistic
        public StatisticController(StatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }
        public ActionResult Index()
        {
            StatisticsViewModel model = _statisticsRepository.AverageRecipeTime();            
            return View(model);
        }
    }
}