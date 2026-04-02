namespace RestoManager.Db.Dtos;

public class RestaurantExpensiveMealsDto
{
    public string Name { get; set; } = string.Empty;
    public List<string> ExpensiveMeals { get; set; } = new();
}
