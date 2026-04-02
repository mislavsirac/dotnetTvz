namespace RestoManager.Db.Dtos;

public class TopMealDto
{
    public string Meal { get; set; } = string.Empty;
    public int OrderedQuantity { get; set; }
    public decimal Revenue { get; set; }
}
