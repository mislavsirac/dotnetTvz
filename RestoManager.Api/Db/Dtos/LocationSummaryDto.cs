namespace RestoManager.Db.Dtos;

public class LocationSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int StaffCount { get; set; }
    public int OrderCount { get; set; }
}
