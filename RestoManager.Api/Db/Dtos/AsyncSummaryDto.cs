namespace RestoManager.Db.Dtos;

public class AsyncSummaryDto
{
    public DateTime GeneratedAt { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}
