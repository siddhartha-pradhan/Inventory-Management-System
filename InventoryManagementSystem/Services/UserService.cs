using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services;

public abstract class UserService : GenericService<User>
{
	/// <summary>
	/// Defining the static user credentials to list out when program is executed for the first time
	/// </summary>
	public const string SeedUsername = "admin";
	public const string SeedPassword = "admin";
	private const string SeedEmail = "admin@admin.com";

	/// <summary>
	/// Defining a static path for all the required files for the service and domain logic
	/// </summary>
	private static string appDataDirectoryPath = UtilityService.GetAppDirectoryPath();
	private static string appUsersFilePath = UtilityService.GetAppUsersFilePath();

	/// <summary>
	/// Defining a static method to automatically create first admin user during the program execution in the first state
	/// </summary>
	public static void SeedUser()
	{

		var users = GetAll(appUsersFilePath).FirstOrDefault(x => x.Role == Role.Admin);

		if (users == null)
		{
            Create(Guid.Empty, SeedUsername, SeedEmail, SeedPassword, Role.Admin);
		}
	}

	/// <summary>
	/// Defining a method to allow login access to a user as per the entered credentials
	/// </summary>
	/// <param name="username">Username in the form of user credentials</param>
	/// <param name="password">Password in the form of user credentials</param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public static User Login(string username, string password)
	{
		var loginErrorMessage = "Invalid username or password.";

		var users = GetAll(appUsersFilePath);

		var user = users.FirstOrDefault(x => x.Username == username.Trim().ToLower());

		if (user == null)
		{
			throw new Exception(loginErrorMessage);
		}

		var passwordIsValid = UtilityService.VerifyHash(password, user.PasswordHash);

		if (!passwordIsValid)
		{
			throw new Exception(loginErrorMessage);
		}

		return user;
	}

	/// <summary>
	/// Defining a method to return a user as per its identifier
	/// </summary>
	/// <param name="id">User's ID</param>
	/// <returns>The user instance for a valid ID</returns>
	public static User GetById(Guid id)
	{
		var user = GetAll(appUsersFilePath).FirstOrDefault(x => x.Id == id);

		return user;
	}

	/// <summary>
	/// Defining a method to create a new user as per the model's properties
	/// </summary>
	/// <param name="userId">User's ID</param>
	/// <param name="username">User's Name</param>
	/// <param name="email">User's Email</param>
	/// <param name="password">User's Password</param>
	/// <param name="role">User's Role</param>
	/// <returns>An updated list of user with a new created user</returns>
	/// <exception cref="Exception"></exception>
	public static List<User> Create(Guid userId, string username, string email, string password, Role role)
	{
		username = username.Trim();
		
		if(username == "" || email == "" || password == "")
		{
			throw new Exception("Please insert correct and valid input for each of the fields.");
		}

		var users = GetAll(appUsersFilePath);

		var usernameExists = users.Any(x => x.Username == username);

		if (usernameExists)
		{
			throw new Exception("Username already exists. Please choose any other username.");
		}

		var numberOfAdmins = users.Count(x => x.Role == Role.Admin);

		if (numberOfAdmins >= 2 && role == Role.Admin)
		{
			throw new Exception("The system already has two admins. Further addition of admin is not allowed.");
		}

		var user = new User()
		{
			Username = username,
			Email = email,
			PasswordHash = UtilityService.HashSecret(password),
			Role = role,
			CreatedBy = userId,
		};

		users.Add(user);

		SaveAll(users, appDataDirectoryPath, appUsersFilePath);

		return users;
	}

	/// <summary>
	/// Defines a user to delete an instance of a user 
	/// </summary>
	/// <param name="id">Valid user id</param>
	/// <returns>An updated list of user without the deleted user</returns>
	/// <exception cref="Exception">For a user that's not found</exception>
	public static List<User> Delete(Guid id)
	{
		var users = GetAll(appUsersFilePath);

		var user = users.FirstOrDefault(x => x.Id == id);

		if (user == null)
		{
			throw new Exception("User not found.");
		}

		users.Remove(user);

		SaveAll(users, appDataDirectoryPath, appUsersFilePath);

		return users;
	}
}
