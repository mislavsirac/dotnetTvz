namespace RestoManager.Model.Orders;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int MealId { get; set; }
    public string MealName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string SpecialInstructions { get; set; } = string.Empty;

    public decimal LineTotal => Quantity * UnitPrice;
}
