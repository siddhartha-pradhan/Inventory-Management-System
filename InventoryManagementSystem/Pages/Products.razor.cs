using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Components;

namespace InventoryManagementSystem.Pages;

public partial class Products
{
    [CascadingParameter] 
    private GlobalState _globalState { get; set; }

    private List<Product> _products { get; set; }
    
    private List<Order> _orders { get; set; }
    
    private bool _showUpsertProductDialog { get; set; }
    
    private bool _showDeleteProductDialog { get; set; }
    
    private bool _showOrderProductDialog { get; set; }
    
    private Product _productModel { get; set; }
    
    private Order _orderModel { get; set; }
    
    private string _dialogTitle { get; set; }
    
    private string _dialogOkLabel { get; set; }
    
    private string _upsertProductErrorMessage { get; set; }
    
    private string _orderProductErrorMessage { get; set; }
    
    private string _deleteProductErrorMessage { get; set; }
    
    private string _tabFilter = "All";
    
    private string _sortBy = "product";
    
    private string _sortDirection = "ascending";
    
    private string _productsPath = UtilityService.GetAppProductsFilePath();

    protected override void OnInitialized()
    {
        _products = ProductService.GetAll(_productsPath);
    }

    private void OpenAddProductDialog()
    {
        _dialogTitle = "Add a new product";

        _dialogOkLabel = "Add";

        _upsertProductErrorMessage = "";

        _productModel = new Product();

        _productModel.Id = Guid.Empty;

        _showUpsertProductDialog = true;
    }

    private void OpenEditProductDialog(Product product)
    {
        _dialogTitle = "Edit an existing product";

        _dialogOkLabel = "Save";

        _upsertProductErrorMessage = "";

        _productModel = product;

        _showUpsertProductDialog = true;
    }

    private void OpenDeleteProductDialog(Product product)
    {
        _dialogTitle = "Delete a product";

        _dialogOkLabel = "Confirm";

        _productModel = product;

        _showDeleteProductDialog = true;
    }

    private void OpenOrderProductDialog(Product product)
    {
        _dialogTitle = "Order a product";

        _dialogOkLabel = "Order";

        _orderProductErrorMessage = "";

        _productModel = product;

        _orderModel = new Order();

        _orderModel.Quantity = 0;

        _showOrderProductDialog = true;
    }

    private void SortByHandler(string sortByName)
    {
        if (_sortBy == sortByName)
        {
            _sortDirection = _sortDirection == "ascending" ? "descending" : "ascending";
        }
        else
        {
            _sortBy = sortByName;

            _sortDirection = "ascending";
        }
    }

    public void TabFilter(string status)
    {
        _tabFilter = status;
    }

    private void SearchProductName(ChangeEventArgs e)
    {
        var searchItem = e.Value.ToString();

        if (!string.IsNullOrEmpty(searchItem) && searchItem.Length > 2)
        {
            _products = ProductService.GetAll(_productsPath)
                .Where(p => p.Title.ToLower().Contains(searchItem.ToLower())).ToList();
        }
        else
        {
            _products = ProductService.GetAll(_productsPath);
        }
    }

    private void OnUpsertProductDialogClose(bool isClosed)
    {
        if (isClosed)
        {
            _showUpsertProductDialog = false;
        }
        else
        {
            try
            {
                _upsertProductErrorMessage = "";

                if (_productModel.Id == Guid.Empty)
                {
                    _products = ProductService.Create(_globalState.CurrentUser.Id, _productModel.Title,
                        _productModel.Quantity);
                }

                else
                {
                    _products = ProductService.Update(_productModel.Id, _productModel.Title, _productModel.Quantity);
                }

                _showUpsertProductDialog = false;
            }
            catch (Exception e)
            {
                _upsertProductErrorMessage = e.Message;

                Console.WriteLine(e.Message);
            }
        }
    }

    private void OnDeleteProductDialogClose(bool isClosed)
    {
        if (isClosed)
        {
            _showDeleteProductDialog = false;

            _productModel = null;
        }
        else
        {
            try
            {
                _deleteProductErrorMessage = "";

                _products = ProductService.Delete(_productModel.Id);

                _showDeleteProductDialog = false;

                _productModel = null;
            }
            catch (Exception e)
            {
                _deleteProductErrorMessage = e.Message;

                Console.WriteLine(e.Message);
            }
        }
    }

    private void OnOrderProductDialogClose(bool isClosed)
    {
        if (isClosed)
        {
            _showOrderProductDialog = false;
        }
        else
        {
            try
            {
                _orderProductErrorMessage = "";

                _orders = OrderService.Create(_globalState.CurrentUser.Id, _productModel.Id, _orderModel.Quantity);

                _showOrderProductDialog = false;
            }
            catch (Exception e)
            {
                _orderProductErrorMessage = e.Message;

                Console.WriteLine(e.Message);
            }
        }
    }
}