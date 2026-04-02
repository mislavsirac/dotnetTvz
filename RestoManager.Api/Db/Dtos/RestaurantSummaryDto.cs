namespace RestoManager.Db.Dtos;

public class RestaurantSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BrandCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<LocationSummaryDto> Locations { get; set; } = new();
    public List<MealSummaryDto> Meals { get; set; } = new();
}
