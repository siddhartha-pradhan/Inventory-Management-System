﻿using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.AspNetCore.Components;

namespace InventoryManagementSystem.Pages;

public partial class Users
{
    [CascadingParameter] private GlobalState _globalState { get; set; }

    private List<User> _users { get; set; }
    private bool _showAddUserDialog { get; set; }
    private bool _showDeleteUserDialog { get; set; }
    private string _addUserErrorMessage { get; set; }
    private string _deleteUserErrorMessage { get; set; }
    private User _userModel { get; set; }
    private string _dialogTitle { get; set; }
    private string _dialogOkLabel { get; set; }
    private Role _userRole { get; set; }
    private string _userPassword { get; set; }
    private string _tabFilter = "All";
    private string _sortBy = "username";
    private string _sortDirection = "ascending";
    private string _usersPath = UtilityService.GetAppUsersFilePath();

    protected override void OnInitialized()
    {
        _users = UserService.GetAll(_usersPath);
    }

    private void RoleChangeHandler(ChangeEventArgs e)
    {
        _userRole = (Role)Enum.Parse(typeof(Role), e.Value.ToString());
    }

    private void OpenAddUserDialog()
    {
        _dialogTitle = "Add a new user";

        _dialogOkLabel = "Add";

        _userModel = new User();

        _userPassword = "";

        _showAddUserDialog = true;
    }

    private void OpenDeleteUserDialog(User user)
    {
        _dialogTitle = "Remove an existing user";

        _dialogOkLabel = "Confirm";

        _userModel = user;

        _showDeleteUserDialog = true;
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

    private void SearchUserName(ChangeEventArgs e)
    {
        var searchItem = e.Value.ToString();

        if (!string.IsNullOrEmpty(searchItem) && searchItem.Length > 2)
        {
            _users = UserService.GetAll(_usersPath).Where(p => p.Username.ToLower().Contains(searchItem.ToLower()))
                .ToList();
        }
        else
        {
            _users = UserService.GetAll(_usersPath);
        }
    }

    private void OnAddUserDialogClose(bool isClosed)
    {
        if (isClosed)
        {
            _showAddUserDialog = false;
        }
        else
        {
            try
            {
                _addUserErrorMessage = "";

                _users = UserService.Create(_globalState.CurrentUser.Id, _userModel.Username, _userModel.Email,
                    _userPassword, _userRole);

                _showAddUserDialog = false;
            }
            catch (Exception e)
            {
                _addUserErrorMessage = e.Message;

                Console.WriteLine(e.Message);
            }
        }
    }

    private void OnDeleteUserDialogClose(bool isClosed)
    {
        if (isClosed)
        {
            _showDeleteUserDialog = false;

            _userModel = null;
        }
        else
        {
            try
            {
                _deleteUserErrorMessage = "";

                _users = UserService.Delete(_userModel.Id);

                _showDeleteUserDialog = false;

                _userModel = null;
            }
            catch (Exception e)
            {
                _deleteUserErrorMessage = e.Message;

                Console.WriteLine(e.Message);
            }
        }
    }
}