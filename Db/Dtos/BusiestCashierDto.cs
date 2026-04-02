namespace RestoManager.Db.Dtos;

public class BusiestCashierDto
{
    public string Cashier { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int OrdersHandled { get; set; }
}
