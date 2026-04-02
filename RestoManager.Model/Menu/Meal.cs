namespace RestoManager.Model.Menu;

public class Meal
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int PreparationTimeMinutes { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime AddedToMenuAt { get; set; }
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
}
