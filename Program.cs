using RestoManager.Db;
using RestoManager.Db.Dtos;
using RestoManager.Model;
using RestoManager.Model.Enums;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Lab1DataStore>();

var app = builder.Build();
var dataStore = app.Services.GetRequiredService<Lab1DataStore>();
var overview = GetOverview(dataStore);
var restaurantSummaries = GetRestaurantSummaries(dataStore);
var restaurantStatistics = GetRestaurantStatistics(dataStore);
var startupSummary = await GetAsyncSummaryAsync(dataStore);
var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true
};

Console.WriteLine("Lab 1 demo data loaded.");
PrintSection("Overview", overview, jsonOptions);
PrintSection("Restaurants", restaurantSummaries, jsonOptions);
PrintSection("Statistics", restaurantStatistics, jsonOptions);
PrintSection("Async summary", startupSummary, jsonOptions);

app.Run();

static OverviewDto GetOverview(Lab1DataStore dataStore)
{
    return new OverviewDto
    {
        RestaurantCount = dataStore.Restaurants.Count,
        LocationCount = dataStore.Restaurants.SelectMany(restaurant => restaurant.Locations).Count(),
        StaffCount = dataStore.Restaurants.SelectMany(restaurant => restaurant.Locations).SelectMany(location => location.StaffMembers).Count(),
        MealCount = dataStore.Restaurants.SelectMany(restaurant => restaurant.Meals).Count(),
        OrderCount = dataStore.Restaurants.SelectMany(restaurant => restaurant.Locations).SelectMany(location => location.Orders).Count()
    };
}

static List<RestaurantSummaryDto> GetRestaurantSummaries(Lab1DataStore dataStore)
{
    return dataStore.Restaurants
        .Select(restaurant => new RestaurantSummaryDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            BrandCode = restaurant.BrandCode,
            CreatedAt = restaurant.CreatedAt,
            Locations = restaurant.Locations.Select(location => new LocationSummaryDto
            {
                Id = location.Id,
                Name = location.Name,
                City = location.City,
                StaffCount = location.StaffMembers.Count,
                OrderCount = location.Orders.Count
            }).ToList(),
            Meals = restaurant.Meals.Select(meal => new MealSummaryDto
            {
                Id = meal.Id,
                Name = meal.Name,
                Category = meal.Category,
                Price = meal.Price,
                IngredientCount = meal.RecipeIngredients.Count
            }).ToList()
        })
        .ToList();
}

static RestaurantStatisticsDto GetRestaurantStatistics(Lab1DataStore dataStore)
{
    return new RestaurantStatisticsDto
    {
        ActiveLocationsByRestaurant = GetActiveLocationsByRestaurantStatistics(dataStore.Restaurants),
        TopMeals = GetTopMealStatistics(dataStore.Restaurants),
        LowStockIngredients = GetLowStockIngredientStatistics(dataStore.Ingredients),
        BusiestCashiers = GetBusiestCashierStatistics(dataStore.Restaurants),
        RestaurantsWithExpensiveMeals = GetRestaurantsWithPremiumMeals(dataStore.Restaurants)
    };
}

static List<ActiveLocationsByRestaurantDto> GetActiveLocationsByRestaurantStatistics(List<Restaurant> restaurants)
{
    return restaurants
        .Select(restaurant => new ActiveLocationsByRestaurantDto
        {
            Restaurant = restaurant.Name,
            OpenLocations = restaurant.Locations.Count(location => location.IsOpen)
        })
        .OrderByDescending(result => result.OpenLocations)
        .ToList();
}

static List<TopMealDto> GetTopMealStatistics(List<Restaurant> restaurants)
{
    return restaurants
        .SelectMany(restaurant => restaurant.Locations)
        .SelectMany(location => location.Orders)
        .SelectMany(order => order.Items)
        .GroupBy(item => item.MealName)
        .Select(group => new TopMealDto
        {
            Meal = group.Key,
            OrderedQuantity = group.Sum(item => item.Quantity),
            Revenue = group.Sum(item => item.LineTotal)
        })
        .OrderByDescending(result => result.OrderedQuantity)
        .ThenByDescending(result => result.Revenue)
        .Take(5)
        .ToList();
}

static List<LowStockIngredientDto> GetLowStockIngredientStatistics(List<Ingredient> ingredients)
{
    return ingredients
        .Where(ingredient => ingredient.CurrentStock <= ingredient.ReorderLevel)
        .OrderBy(ingredient => ingredient.CurrentStock)
        .Select(ingredient => new LowStockIngredientDto
        {
            Name = ingredient.Name,
            CurrentStock = ingredient.CurrentStock,
            ReorderLevel = ingredient.ReorderLevel,
            Unit = ingredient.Unit
        })
        .ToList();
}

static List<BusiestCashierDto> GetBusiestCashierStatistics(List<Restaurant> restaurants)
{
    var allLocations = restaurants.SelectMany(restaurant => restaurant.Locations).ToList();

    return allLocations
        .SelectMany(location => location.Orders, (location, order) => new { location, order })
        .Join(
            allLocations.SelectMany(location => location.StaffMembers),
            left => left.order.CashierId,
            right => right.Id,
            (left, cashier) => new
            {
                Cashier = cashier.FullName,
                Location = left.location.Name
            })
        .GroupBy(result => new { result.Cashier, result.Location })
        .Select(group => new BusiestCashierDto
        {
            Cashier = group.Key.Cashier,
            Location = group.Key.Location,
            OrdersHandled = group.Count()
        })
        .OrderByDescending(result => result.OrdersHandled)
        .ToList();
}

static List<RestaurantExpensiveMealsDto> GetRestaurantsWithPremiumMeals(List<Restaurant> restaurants)
{
    return restaurants
        .Where(restaurant => restaurant.Meals.Any(meal => meal.Price > 12m))
        .Select(restaurant => new RestaurantExpensiveMealsDto
        {
            Name = restaurant.Name,
            ExpensiveMeals = restaurant.Meals
                .Where(meal => meal.Price > 12m)
                .OrderByDescending(meal => meal.Price)
                .Select(meal => meal.Name)
                .ToList()
        })
        .ToList();
}

static async Task<AsyncSummaryDto> GetAsyncSummaryAsync(Lab1DataStore dataStore)
{
    await Task.Delay(300);

    return new AsyncSummaryDto
    {
        GeneratedAt = DateTime.UtcNow,
        PendingOrders = dataStore.Restaurants
            .SelectMany(restaurant => restaurant.Locations)
            .SelectMany(location => location.Orders)
            .Count(order => order.Status is OrderStatus.New or OrderStatus.InPreparation),
        CompletedOrders = dataStore.Restaurants
            .SelectMany(restaurant => restaurant.Locations)
            .SelectMany(location => location.Orders)
            .Count(order => order.Status == OrderStatus.Completed),
        TotalRevenue = dataStore.Restaurants
            .SelectMany(restaurant => restaurant.Locations)
            .SelectMany(location => location.Orders)
            .Where(order => order.Status != OrderStatus.Cancelled)
            .Sum(order => order.TotalAmount)
    };
}

static void PrintSection<T>(string title, T data, JsonSerializerOptions jsonOptions)
{
    Console.WriteLine();
    Console.WriteLine(title);
    Console.WriteLine(new string('=', title.Length));
    Console.WriteLine(JsonSerializer.Serialize(data, jsonOptions));
}
