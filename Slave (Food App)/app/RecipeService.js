/// <reference path="app.js" />


app.service("RecipeService", function ($http,$rootScope) {
    var Recipes = [];
    var Ingredients = [];
    var baseUrl = $rootScope.baseUrl;
    
    var getIngredients = function () {
        return Ingredients;
    }

    var setIngredients = function (data) {
        Ingredients = data;
    }

    var getRecipes = function () {
        return Recipes;
    }

    var setRecipes = function (data) {
        Recipes = data;
    }
        

    return {
        getRecipes: getRecipes,
        setRecipes: setRecipes,
        findByName: findByName,
        getIngredients: getIngredients,
        setIngredients: setIngredients
    };
});