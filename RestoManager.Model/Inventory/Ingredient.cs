using RestoManager.Model.Menu;

namespace RestoManager.Model.Inventory;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public IngredientUnit Unit { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal ReorderLevel { get; set; }
    public decimal CaloriesPer100Units { get; set; }
    public DateTime LastDeliveryAt { get; set; }
    public bool IsAllergen { get; set; }
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
}
