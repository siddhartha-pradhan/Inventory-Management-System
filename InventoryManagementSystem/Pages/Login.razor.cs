using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Components;

namespace InventoryManagementSystem.Pages;

public partial class Login
{
    [CascadingParameter] 
    private GlobalState _globalState { get; set; }

    private string _username { get; set; }
    
    private string _password { get; set; }
    
    private string _errorMessage = "";

    protected override void OnInitialized()
    {
        _globalState.CurrentUser = null;

        try
        {
            UserService.Login(UserService.SeedUsername, UserService.SeedPassword);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message;
            Console.WriteLine(e.Message);
        }
    }

    private void LoginHandler()
    {
        try
        {
            var user = _globalState.CurrentUser = UserService.Login(_username, _password);

            if (user != null)
            {
                NavManager.NavigateTo("/home");
            }
        }
        catch (Exception e)
        {
            _errorMessage = e.Message;
            Console.WriteLine(e);
        }
    }
}