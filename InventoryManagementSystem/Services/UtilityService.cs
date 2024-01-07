using System.Security.Cryptography;

namespace InventoryManagementSystem.Services; 

public abstract class UtilityService 
{
	private const char _segmentDelimiter = ':';

	public static string HashSecret(string input)
	{
		var saltSize = 16;
		var iterations = 100_000;
		var keySize = 32;
		var algorithm = HashAlgorithmName.SHA256;
		var salt = RandomNumberGenerator.GetBytes(saltSize);
		var hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

		var result = string.Join(
			_segmentDelimiter,
			Convert.ToHexString(hash),
			Convert.ToHexString(salt),
			iterations,
			algorithm
		);

		return result;
	}

	public static bool VerifyHash(string input, string hashString)
	{
		var segments = hashString.Split(_segmentDelimiter);
		var hash = Convert.FromHexString(segments[0]);
		var salt = Convert.FromHexString(segments[1]);
		var iterations = int.Parse(segments[2]);
		var algorithm = new HashAlgorithmName(segments[3]);
		var inputHash = Rfc2898DeriveBytes.Pbkdf2(
			input,
			salt,
			iterations,
			algorithm,
			hash.Length
		);

		return CryptographicOperations.FixedTimeEquals(inputHash, hash);
	}

	/// <summary>
	/// Initializing a method so as to retrieve the directory path to store all the records and logs
	/// </summary>
	/// <returns>Path of the directory that holds all the application data</returns>
	public static string GetAppDirectoryPath()
	{
		return @"D:\Learning\C#\Projects\InventoryManagementSystem\InventoryManagementSystem\wwwroot\data";
	}

	public static string GetAppUsersFilePath()
	{
		return Path.Combine(GetAppDirectoryPath(), "users.json");
	}

	public static string GetAppProductsFilePath()
	{
		return Path.Combine(GetAppDirectoryPath(), "products.json");
	}

	public static string GetAppOrdersFilePath()
	{
		return Path.Combine(GetAppDirectoryPath(), "orders.json");
	}
}