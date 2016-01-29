/// <reference path="../app.js" />


app.controller("RecipeController", function ($scope, RecipeService, $routeParams,$http,$rootScope) {
    $scope.Name = "";
    $scope.Recipe = null;
    $scope.selectedIngredients = [];
    $scope.Recipes = [];

    var baseUrl = $rootScope.baseUrl;

    $scope.initData = function()
    {
        $scope.Name = $routeParams.name;
        var rec = RecipeService.findByName($scope.Name);

        if (rec == null) {
            var names = [$scope.Name];
            $http.post(baseUrl + "api/recipe/getbyname", names).success(function (data) {                
                $scope.Recipe = data;                
            })            
        }
        else {
            $scope.Recipe = rec;            
        }


        var ings = localStorage.getItem("selectedIngredients");
        if(ings!=null)
        {
            $scope.selectedIngredients = JSON.parse(ings);            
        }

        window.scroll(0, 0);

    }

    $scope.isInSelected = function (item) {
        var filter = $scope.selectedIngredients.filter(function (el) {
            return el.Name == item.Name;
        });

        if (filter.length > 0) {
            return true;
        }
        
        return false;
    }

    
});