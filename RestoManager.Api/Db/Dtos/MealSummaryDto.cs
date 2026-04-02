namespace RestoManager.Db.Dtos;

public class MealSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int IngredientCount { get; set; }
}
