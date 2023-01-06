using System.Text.Json;

namespace InventoryManagementSystem.Services; 

public class GenericService <T> where T : class
{
	/// <summary>
	/// The method defined to retrieve all the records from a json file by deserializing it to an actual list
	/// </summary>
	/// <param name="filePath">The path of the item that is to be deserialized or converted to a list</param>
	/// <returns>A list of objects as per the json string conversion</returns>
	public static List<T> GetAll(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return new List<T>();
		}

		var json = File.ReadAllText(filePath);

		var result = JsonSerializer.Deserialize<List<T>>(json);

		return result;
	}

	/// <summary>
	/// Defines a method to save all the list of items into a serialized json string
	/// </summary>
	/// <param name="entity">List of items</param>
	/// <param name="directoryPath">Path of the directory storage</param>
	/// <param name="filePath">Path of the file to be stored</param>
	public static void SaveAll(List<T> entity, string directoryPath, string filePath)
	{
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}

		var json = JsonSerializer.Serialize(entity);

		File.WriteAllText(filePath, json);
	}
}