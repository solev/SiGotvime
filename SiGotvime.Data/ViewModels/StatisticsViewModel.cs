using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.ViewModels
{
    public class StatisticsViewModel
    {
        public decimal AverageRecipeTime { get; set; }
        public double MinimumRecipeTime { get; set; }
        public double MaximumRecipeTime { get; set; }
    }
}
