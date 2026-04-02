using RestoManager.Model.Inventory;

namespace RestoManager.Db.Dtos;

public class LowStockIngredientDto
{
    public string Name { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public decimal ReorderLevel { get; set; }
    public IngredientUnit Unit { get; set; }
}
