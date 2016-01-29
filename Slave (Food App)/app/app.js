/// <reference path="../Scripts/angular.js" />

var app = angular.module("FoodApp", ['ngRoute', 'ngAnimate', 'ngMaterial', 'me-lazyload']).run(function ($rootScope, $window) {
    $rootScope.baseUrl = "http://localhost:61820/";
    $rootScope.baseUrl = "http://portal-baranjerecepti.azurewebsites.net/";
       
})

app.config(function ($routeProvider) {
    $routeProvider
    .when("/", {
        templateUrl: "app/Home/home.html",
        controller: "HomeController"
    })
    .when("/recipe/:name", {
        templateUrl: "app/Recipe/recipe.html",
        controller: "RecipeController"
    })
});

app.factory("DataFactory", function () {
    var svc = {};
    svc.selectedIngredients = [];
    svc.Recipes = [];
    svc.Ingredients = [];
    svc.Categories = [];
    svc.SearchedRecipes = [];

    svc.getSelectedIngredients = function () {
        return svc.selectedIngredients;
    }

    svc.getIngredients = function()
    {
        return svc.Ingredients;
    }

    svc.getRecipes = function()
    {
        return svc.Recipes;
    }

    svc.setIngredients = function(data)
    {
        svc.Ingredients = data;
    }
    svc.setRecipes = function(data)
    {
        angular.copy(data, svc.Recipes);
    }

    svc.addIngredient = function(ingredient)
    {
        if (svc.selectedIngredients.indexOf(ingredient) != -1)
            return;
        svc.selectedIngredients.push(ingredient);        
    }

    svc.findIgredient = function (name) {
        var result = svc.Ingredients.filter(function (el) {
            return el.Name.toLowerCase() == name.toLowerCase();
        });

        if (result.length > 0) {
            return result[0];
        }
        else {
            var obj = null;
            return obj;
        }
    }

    svc.removeIngredient = function(item)
    {
        var arr = svc.selectedIngredients.filter(function (el) {
            return el.ID != item.ID;
        });

        angular.copy(arr, svc.selectedIngredients);
    }

    return svc;
});