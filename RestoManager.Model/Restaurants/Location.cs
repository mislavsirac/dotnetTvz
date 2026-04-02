using RestoManager.Model.Orders;
using RestoManager.Model.Staff;

namespace RestoManager.Model.Restaurants;

public class Location
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsOpen { get; set; }
    public DateTime OpenedAt { get; set; }
    public List<StaffMember> StaffMembers { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}
