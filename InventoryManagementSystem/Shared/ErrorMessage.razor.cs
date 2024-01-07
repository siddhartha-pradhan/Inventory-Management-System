using Microsoft.AspNetCore.Components;

namespace InventoryManagementSystem.Shared;

public partial class ErrorMessage
{
    [Parameter] public string Type { get; set; }

    [Parameter] public string Message { get; set; }
}