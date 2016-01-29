/// <reference path="../app.js" />

app.controller("HomeController", function ($scope, $http, $rootScope, $location, $interval, DataFactory) {
    var url = $rootScope.baseUrl;

    $scope.alphabet = ['А', 'Б', 'В', 'Г', 'Д', 'Ѓ', 'Е', 'Ж', 'З', 'Ѕ', 'И', 'Ј', 'К', 'Л', 'Љ', 'М', 'Н', 'Њ', 'О', 'П', 'Р', 'С', 'Т', 'Ќ', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Џ', 'Ш'];
    $scope.videoUrl = "http://localhost:61820/Content/video/";
    $scope.videoUrl = "http://portal-baranjerecepti.azurewebsites.net/Content/video/";

    //Ingredients
    $scope.Ingredients = DataFactory.Ingredients;
    $scope.selectedIngredients = DataFactory.selectedIngredients;
    $scope.loadingIngridients = true;
    $scope.ingredientSearchText = "";
    $scope.selectedIngredient = {};

    //Recipes
    $scope.Recipes = DataFactory.Recipes;
    $scope.SearchedRecipes = DataFactory.SearchedRecipes;
    $scope.SelectedRecipe = {};
    $scope.loadingRecipes = false;
    $scope.loadedRecipes = false;
    $scope.loadingMore = false;
    $scope.activeTab = 2;
    $scope.search = false;
    $scope.RecipesFromTab = 2;
    //Statuses
    $scope.Statuses = [];
    $scope.currentStatus = "";

    //Categories
    $scope.Categories = [];
    $scope.selectedCategory = {};
    var idx = 0;
    $scope.circle;

    function init() {
        $scope.Ingredients.length = 0;
        if ($scope.Statuses.length == 0) {
            $http.get(url + "api/ingredient/getStatuses").success(function (data) {
                angular.copy(data, $scope.Statuses);                
                $scope.currentStatus = $scope.Statuses[0];
                $scope.circulate();
            })
        }
        else {
            $scope.circulate();
        }
        $http.get(url + "api/ingredient/videourl").success(function (data) {
            $scope.videoUrl += data;
            $(".hero").background({
                source: {
                    poster: "",
                    mp4: $scope.videoUrl,
                    webm:$scope.videoUrl
                }
            });            
        });

        if ($scope.Ingredients.length == 0) {
            $http.get(url + "api/ingredient/getall").success(function (data, status) {
                $scope.loadingIngridients = false;
                angular.copy(data, DataFactory.Ingredients);
            });
        }
        else {
            $scope.loadingIngridients = false;
        }


        $scope.loadingRecipes = true;
        taken = 0;
        if ($scope.Recipes.length == 0) {
            $http.post(url + "api/recipe/gethomerecipes", taken).success(function (data) {
                angular.forEach(data, function (value, key) {
                    DataFactory.Recipes.push(value);
                })

                $scope.loadingRecipes = false;
                $scope.loadedRecipes = true;
            });
        }

        if ($scope.Categories == 0) {
            $http.get(url + "api/tags/getall").success(function (data) {
                angular.copy(data, DataFactory.Categories);                
            })
        }
    }

    $scope.getImageBinary = function(item)
    {
        var recipeID = item.ID;
        console.log(item);
        $http.post(url + "api/recipe/GetImageBinary", recipeID).success(function (data) {
            return data;
        })
    }

    $scope.getMatches = function()
    {
        var filter = $scope.Ingredients.filter(function (el) {
            return el.Name.toLowerCase().indexOf($scope.ingredientSearchText.toLowerCase()) != -1;
        })
        
        var result = [];
        angular.forEach(filter, function (value, key) {
            value.itemName = value.Name;
            result.push(value);
        })        
        return result;
    }

    $scope.circulate = function () {
        
        if (angular.isDefined($scope.circle)) return;
        $scope.circle = $interval(function () {
            $(".messages").fadeOut(1000, "swing", function () {
                idx++;
                idx = idx % $scope.Statuses.length;
                $scope.currentStatus = null;
                $scope.currentStatus = $scope.Statuses[idx];
                $scope.$apply();
                $(".messages").fadeIn(1000);
            });
        }, 8000);
    }

    $scope.ingredientSelect = function () {
        if ($scope.selectedIngredient.Name == undefined)
        {
            return;
        }
        var filtered = $scope.selectedIngredients.filter(function (el) {
            return el.ID == $scope.selectedIngredient.ID;
        });
        $scope.ingredientSearchText = "";
        if (filtered.length > 0)
            return;
        $scope.selectedIngredients.push($scope.selectedIngredient);
        
    }
    //$scope.$watch('selectedIngredient', function (newValue, oldValue) {
    //    var filtered = $scope.selectedIngredients.filter(function (obj) {
    //        return obj.ID == item.ID;
    //    });
    //    $scope.ingredientSearchText = "";
    //    if (filtered.length > 0)
    //        return;

    //    $scope.selectedIngredients.push($scope.selectedIngredient);
    //})

    $scope.activateTab = function (tab) {
        $scope.activeTab = tab;
        if (tab == 1) {
            $scope.Categories = null;
            $scope.Categories = [];
            angular.copy(DataFactory.Categories, $scope.Categories);
        }
        else if (tab == 2) {
            if ($scope.RecipesFromTab != 2) {
                
                $scope.loadedRecipes = false;
                $scope.loadingRecipes = true;
                $scope.search = false;
                var taken = 0;
                $http.post(url + "api/recipe/gethomerecipes", taken).success(function (data) {
                    DataFactory.setRecipes(data);
                    $scope.loadingRecipes = false;
                    $scope.loadedRecipes = true;
                });
                $scope.RecipesFromTab = tab;
            }
        }
        else if (tab == 3) {
            if ($scope.RecipesFromTab != 3) {
                $scope.loadedRecipes = false;
                $scope.loadingRecipes = true;
                $scope.search = false;
                var taken = 0;
                $http.post(url + "api/recipe/geteasiest",taken).success(function (data) {
                    angular.copy(data, DataFactory.Recipes);
                    $scope.loadingRecipes = false;
                    $scope.loadedRecipes = true;
                })

                $scope.RecipesFromTab = tab;
            }
        }
    }

    init();
    $scope.selectItem = function (item) {

        var filtered = $scope.selectedIngredients.filter(function (obj) {
            return obj.ID == item.ID;
        });

        if (filtered.length > 0)
            return;

        $scope.selectedIngredients.push(item);
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


    $scope.removeItem = function (item) {
        DataFactory.removeIngredient(item);
    }

    $scope.searchRecipes = function () {
        $scope.loadingRecipes = true;
        $scope.loadedRecipes = false;
        $scope.search = true;
        
        var model = { ingredients: $scope.selectedIngredients, taken: 0 };

        $http.post(url + "api/recipe/search", model).success(function (data) {
            angular.copy(data, DataFactory.SearchedRecipes);
            $scope.loadingRecipes = false;
            $scope.loadedRecipes = true;
        })

    }

    $scope.getByCategory = function (item) {
        $scope.loadingRecipes = true;
        $scope.loadedRecipes = false;
        var Categories = [item.ID];
        var model = { Categories: Categories, taken: 0 };
        $scope.selectedCategory = item;
        $scope.search = true;
        $http.post(url + "api/recipe/getbycategory", model).success(function (data) {
            angular.copy(data, DataFactory.SearchedRecipes);
            $scope.loadingRecipes = false;
            $scope.loadedRecipes = true;
        })
    }

    $scope.goTo = function (recipe) {
        $scope.SelectedRecipe = recipe;
        $scope.$apply();
        setTimeout(function () {
            var dialogContent = $("#recipeContent").html();

            $.Dialog({
                overlay: true,
                shadow: true,
                flat: true,
                title: recipe.Title,
                content: dialogContent,
                width: '75%',
                height: '500',
                onShow: function () {
                    var w = window,
                        d = document,
                        e = d.documentElement,
                        g = d.getElementsByTagName('body')[0],
                        x = w.innerWidth || e.clientWidth || g.clientWidth,
                        y = w.innerHeight || e.clientHeight || g.clientHeight;

                    y -= 150;
                    $(".metro .window.flat .content").css("max-height", y + "px");

                }
            });

        }, 200)

        console.log(dialogContent);
    }
    $scope.loadMore = function () {
        $scope.loadingMore = true;
        if ($scope.activeTab == 2 && !$scope.search) {
            taken = $scope.Recipes.length;
            $http.post(url + "api/recipe/gethomerecipes", taken).success(function (data) {
                for (var i = 0; i < data.length; i++) {
                    $scope.Recipes.push(data[i]);
                }
                $scope.loadingMore = false;
            });
        }
        else if($scope.activeTab == 3 && !$scope.search)
        {
            taken = $scope.Recipes.length;
            $http.post(url + "api/recipe/GetEasiest", taken).success(function (data) {
                for (var i = 0; i < data.length; i++) {
                    $scope.Recipes.push(data[i]);
                }
                $scope.loadingMore = false;
            });
        }
        else if($scope.activeTab == 1 && $scope.search)
        {
            
            var Categories = $scope.selectedCategory.ID;
            var model = { Categories: Categories, taken: DataFactory.SearchedRecipes.length };            
            
            $http.post(url + "api/recipe/getbycategory", model).success(function (data) {
                angular.forEach(data, function (value, key) {
                    DataFactory.SearchedRecipes.push(value);
                });
                $scope.loadingMore = false;
            })
        }
        else {
            var model = { ingredients: $scope.selectedIngredients, taken: DataFactory.SearchedRecipes.length };
            $http.post(url + "api/recipe/search", model).success(function (data) {
                
                angular.forEach(data, function (value,key) {
                    DataFactory.SearchedRecipes.push(value);
                })
               
                $scope.loadingRecipes = false;
                $scope.loadedRecipes = true;
                $scope.loadingMore = false;

            })
        }

    }

    // OLD STUFF    

    $scope.changeAlpha = function (item) {
        var idx = $scope.alphabet.indexOf(item);
        $scope.selectedIndex = idx;


        $scope.filteredIngredients = $scope.Ingredients.filter(function (el) {
            return el.Name.toUpperCase().charAt(0) == $scope.alphabet[$scope.selectedIndex];
        });
    }

    $scope.$on('$destroy', function () { $interval.cancel($scope.circle); });

    $scope.newSearch = function () {
        $scope.loadedRecipes = false;
        $scope.Recipes = null;
        $scope.Recipes = [];
    }


    $scope.filterInput = function (obj) {
        if ($scope.ingredientSearchText == "") {

            $scope.filteredIngredients = [];
            $scope.selectedIndex = 0;
            return;
        }
        $scope.selectedIndex = -1;
        $scope.filteredIngredients = $scope.Ingredients.filter(function (el) {
            return el.Name.toLowerCase().indexOf($scope.ingredientSearchText.toLowerCase()) >= 0;
        })
    }




})
