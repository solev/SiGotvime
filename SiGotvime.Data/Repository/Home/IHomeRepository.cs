using SiGotvime.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public interface IHomeRepository
    {
        HomePageViewModel GetHomePageData();
    }
}
