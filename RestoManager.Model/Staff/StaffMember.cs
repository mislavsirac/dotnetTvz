using RestoManager.Model.Staff;

namespace RestoManager.Model.Staff;

public class StaffMember
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public StaffRole Role { get; set; }
    public DateTime EmploymentStartDate { get; set; }
    public bool IsActive { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
