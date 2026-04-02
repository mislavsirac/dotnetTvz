using RestoManager.Model.Inventory;

namespace RestoManager.Model.Menu;

public class RecipeIngredient
{
    public int MealId { get; set; }
    public int IngredientId { get; set; }
    public decimal Quantity { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Meal? Meal { get; set; }
    public Ingredient? Ingredient { get; set; }
}
