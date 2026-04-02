using RestoManager.Model.Menu;

namespace RestoManager.Model.Restaurants;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BrandCode { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string HeadquartersCity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public List<Location> Locations { get; set; } = new();
    public List<Meal> Meals { get; set; } = new();
}
