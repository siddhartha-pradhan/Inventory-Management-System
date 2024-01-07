using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Components;

namespace InventoryManagementSystem.Pages;

public partial class Index
{
    [CascadingParameter] private GlobalState _globalState { get; set; }

    protected override void OnInitialized()
    {
        NavManager.NavigateTo(_globalState.CurrentUser == null ? "/login" : "/home");
    }
}