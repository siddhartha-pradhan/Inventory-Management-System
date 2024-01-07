using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Pages;

public partial class Statistics
{
    private List<Order> _orders { get; set; }
    
    private List<OrderQuantity> _sales { get; set; }
    
    private string _tabFilter = "All";
    
    private string _sortBy = "product";
    
    private string _sortDirection = "ascending";
    
    private string _ordersPath = UtilityService.GetAppOrdersFilePath();

    protected override void OnInitialized()
    {
        _orders = OrderService.GetAll(_ordersPath).Where(x => x.OrderStatus != Status.Pending).ToList();
        _sales = OrderService.GetOrderedQuantity();
    }

    public void TabFilter(string status)
    {
        _tabFilter = status;
    }
}