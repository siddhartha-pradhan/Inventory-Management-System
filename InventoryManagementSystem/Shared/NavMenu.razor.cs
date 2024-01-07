using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Components;

namespace InventoryManagementSystem.Shared;

public partial class NavMenu
{
    [Parameter] public string Username { get; set; }

    [Parameter] public Role UserRole { get; set; }

    [Parameter] public EventCallback LogoutHandler { get; set; }

    private bool _collapseNavMenu { get; set; } = true;
    private string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }
}