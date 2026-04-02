using RestoManager.Model.Orders;

namespace RestoManager.Model.Orders;

public class Order
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public int CashierId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public OrderStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(item => item.LineTotal);
}
