using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Shared;

public partial class MainLayout
{
    private GlobalState _globalState = new();

    protected override void OnInitialized()
    {
        UserService.SeedUser();
    }

    private void LogoutHandler()
    {
        _globalState.CurrentUser = null;

        NavManager.NavigateTo("/login");
    }
}