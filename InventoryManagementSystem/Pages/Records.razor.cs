using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Pages;

public partial class Records
{
    private List<Order> _orders { get; set; }
    
    private string _ordersPath = UtilityService.GetAppOrdersFilePath();

    protected override void OnInitialized()
    {
        _orders = OrderService.GetAll(_ordersPath).Where(x => x.OrderStatus == Status.Approved).ToList();
    }

    private void FilterByMonth(string month)
    {
        OnInitialized();
        _orders = _orders.Where(x => x.ActivityAt.ToString("MMM") == month).ToList();
    }
}