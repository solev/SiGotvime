﻿using SiGotvime.Data.Result_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public interface IHistoryRepository
    {
        RecipesResultModel GetPreviousRecipesOfTheDay(int page, int pageSize);
        bool InsertRecipeOfTheDay(int recipeID);
    }
}
