namespace RestoManager.Db.Dtos;

public class ActiveLocationsByRestaurantDto
{
    public string Restaurant { get; set; } = string.Empty;
    public int OpenLocations { get; set; }
}
