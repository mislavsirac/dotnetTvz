namespace RestoManager.Db.Dtos;

public class RestaurantStatisticsDto
{
    public List<ActiveLocationsByRestaurantDto> ActiveLocationsByRestaurant { get; set; } = new();
    public List<TopMealDto> TopMeals { get; set; } = new();
    public List<LowStockIngredientDto> LowStockIngredients { get; set; } = new();
    public List<BusiestCashierDto> BusiestCashiers { get; set; } = new();
    public List<RestaurantExpensiveMealsDto> RestaurantsWithExpensiveMeals { get; set; } = new();
}
