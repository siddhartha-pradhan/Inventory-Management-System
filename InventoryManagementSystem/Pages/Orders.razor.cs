using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Components;

namespace InventoryManagementSystem.Pages;

public partial class Orders
{
    [CascadingParameter] 
    private GlobalState _globalState { get; set; }

    private List<Order> _orders { get; set; }
    
    private Order _orderModel { get; set; }
    
    private bool _showApproveOrderDialog { get; set; }
    
    private bool _showRejectOrderDialog { get; set; }
    
    private string _dialogTitle { get; set; }
    
    private string _dialogOkLabel { get; set; }
    
    private string _approveOrderErrorMessage { get; set; }
    
    private string _rejectOrderErrorMessage { get; set; }
    
    private string _tabFilter = "All";
    
    private string _sortBy = "product";
    
    private string _sortDirection = "ascending";
    
    private string _ordersPath = UtilityService.GetAppOrdersFilePath();

    protected override void OnInitialized()
    {
        _orders = OrderService.GetAll(_ordersPath);
    }

    private void OpenApproveOrderDialog(Order order)
    {
        _dialogTitle = "Approve an order";

        _dialogOkLabel = "Confirm";

        _approveOrderErrorMessage = "";

        _orderModel = order;

        _showApproveOrderDialog = true;
    }

    private void OpenRejectOrderDialog(Order order)
    {
        _dialogTitle = "Reject a requested order";

        _dialogOkLabel = "Confirm";

        _rejectOrderErrorMessage = "";

        _orderModel = order;

        _showRejectOrderDialog = true;
    }

    private void SortByHandler(string sortByIndex)
    {
        if (_sortBy == sortByIndex)
        {
            _sortDirection = _sortDirection == "ascending" ? "descending" : "ascending";
        }
        else
        {
            _sortBy = sortByIndex;

            _sortDirection = "ascending";
        }
    }

    public void TabFilter(string status)
    {
        _tabFilter = status;
    }

    private void OnApproveOrderDialogClose(bool isClosed)
    {
        if (isClosed)
        {
            _showApproveOrderDialog = false;
        }
        else
        {
            try
            {
                _orders = OrderService.Update(_orderModel.Id, _globalState.CurrentUser.Id, Status.Approved);

                _showApproveOrderDialog = false;
            }
            catch (Exception e)
            {
                _approveOrderErrorMessage = e.Message;

                Console.WriteLine(e.Message);
            }
        }
    }

    private void OnRejectOrderDialogClose(bool isClosed)
    {
        if (isClosed)
        {
            _showRejectOrderDialog = false;
        }
        else
        {
            try
            {
                _orders = OrderService.Update(_orderModel.Id, _globalState.CurrentUser.Id, Status.Rejected);

                _showRejectOrderDialog = false;
            }
            catch (Exception e)
            {
                _rejectOrderErrorMessage = e.Message;

                Console.WriteLine(e.Message);
            }
        }
    }
}